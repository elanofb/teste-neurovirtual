using System;
using System.Linq;
using BLL.LogsAlteracoes;
using BLL.Services;
using DAL.Entities;
using DAL.Financeiro;
using DAL.LogsAlteracoes;

namespace BLL.Financeiro.Services {

    public class TituloReceitaPagamentoAlteracaoDadosBL : DefaultBL, ITituloReceitaPagamentoAlteracaoDadosBL {
        
        //Atributos
        private ILogAlteracaoBL _LogAlteracaoBL;

        //Propriedades
        private ILogAlteracaoBL OLogAlteracaoBL => _LogAlteracaoBL = _LogAlteracaoBL ?? new LogAlteracaoBL();

        /// <summary>
        /// Alteração de campos
        /// </summary>
        public UtilRetorno alterarCampo(int id, string nomeCampo, string novoValor, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "", string justificativa = "") {
            
            UtilRetorno ORetorno = new UtilRetorno();
        
            var OTituloReceitaPagamento = db.TituloReceitaPagamento.condicoesSeguranca().FirstOrDefault(x => x.id == id);

            if (OTituloReceitaPagamento == null) {

                ORetorno.flagError = true;

                ORetorno.listaErros.Add("Não foi possível localizar o registro");

                return ORetorno;
            }
            
            if (OTituloReceitaPagamento.dtExclusao.HasValue) {
                
                ORetorno.flagError = true;

                ORetorno.listaErros.Add("Não é possível alterar um registro que esta excluído");

                return ORetorno;
            }

            var OLogAlteracao = new LogAlteracao();

            OLogAlteracao.idEntidadeReferencia = EntityTypesConst.TITULO_RECEITA_PAGAMENTO;

            OLogAlteracao.idReferencia = id;

            OLogAlteracao.valorNovo = novoValor;

            OLogAlteracao.nomeCampoAlterado = nomeCampo;

            OLogAlteracao.justificativa = justificativa.abreviar(100);

            OLogAlteracao.nomeCampoDisplay = nomeCampoDisplay;

            OLogAlteracao.oldValueSelect = oldValueSelect;

            OLogAlteracao.newValueSelect = novoValor.isEmpty() ? null : novoValor;

            ORetorno = this.alterarCampo(OTituloReceitaPagamento, OLogAlteracao);

            if (ORetorno.flagError) {
                return ORetorno;
            }

            ORetorno.flagError = false;

            ORetorno.listaErros.Add("Registro alterado com sucesso");

            ORetorno.info = OTituloReceitaPagamento.id;

            return ORetorno;
        }

        /// <summary>
        /// Faz a alteração de qualquer campo informado
        /// </summary>
        private UtilRetorno alterarCampo(TituloReceitaPagamento OTituloReceitaPagamento, LogAlteracao OLog) {
                
            var ORetorno = UtilRetorno.newInstance(true);

//            var listaCamposEditaveis = new[] {
//                "descricao", "idContaBancaria", "idCentroCusto", "idMacroConta", "idCategoria",
//                "nroNotaFiscal", "nroContrato", "codigoBoleto", "idModoPagamento", "idContaBancariaFavorecida",
//                "documentoPessoaCredor","nomePessoaCredor","nroTelPrincipalCredor"
//            };
//
//            if (!listaCamposEditaveis.Contains(OLog.nomeCampoAlterado)) {
//
//                return UtilRetorno.newInstance(true, "O dado informado não pode ser alterado.");
//            }

            OLog.valorAntigo = OTituloReceitaPagamento.alterarValorCampo(OLog.nomeCampoAlterado, OLog.valorNovo);

            if (OLog.valorAntigo == null) {

                ORetorno.listaErros.Add("O valor informado é inválido");

                return ORetorno;
            }

            var Retorno = db.validateAndSave();

            if (Retorno.flagError){

                return Retorno;

            }

            OLog.nomeCampoAlterado = OLog.nomeCampoAlterado.abreviar(255);

            OLog.nomeCampoDisplay = OLog.nomeCampoDisplay.abreviar(255);

            OLog.valorNovo = OTituloReceitaPagamento.getValorCampo(OLog.nomeCampoAlterado).removeTags().abreviar(255);

            OLog.valorAntigo = OLog.valorAntigo.removeTags().abreviar(255);

            OLog.oldValueSelect = OLog.valorAntigo.isEmpty() ? null : OLog.oldValueSelect.removeTags().abreviar(255);

            OLogAlteracaoBL.salvar(OLog);

            ORetorno.flagError = false;

            return ORetorno;
        }
    }
}
