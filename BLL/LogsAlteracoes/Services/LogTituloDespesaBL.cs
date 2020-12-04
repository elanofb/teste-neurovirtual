using System;
using System.Linq;
using BLL.Services;
using DAL.Entities;
using DAL.Financeiro;
using DAL.LogsAlteracoes;

namespace BLL.LogsAlteracoes {

    public class LogTituloDespesaBL : DefaultBL, ILogTituloDespesaBL {

        //Atributo
        private ILogAlteracaoBL _LogAlteracaoBL;

        //Propriedade
        private ILogAlteracaoBL OLogAlteracaoBL => _LogAlteracaoBL = _LogAlteracaoBL ?? new LogAlteracaoBL();

        
        public UtilRetorno alterarCampo(int id, string nomeCampo, string novoValor, string justificativa, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "") {

            UtilRetorno ORetorno = new UtilRetorno();

            var OTituloDespesa = db.TituloDespesa.condicoesSeguranca().FirstOrDefault(x => x.id == id);
            if (OTituloDespesa == null) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o registro");
                return ORetorno;
            }

            if (OTituloDespesa.dtExclusao.HasValue) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não é possível alterar um registro que esta excluído");
                return ORetorno;
            }

            var OLogAlteracao = new LogAlteracao();
            OLogAlteracao.idEntidadeReferencia = EntityTypesConst.TITULO_DESPESA;
            OLogAlteracao.idReferencia = id;
            OLogAlteracao.valorNovo = novoValor;
            OLogAlteracao.nomeCampoAlterado = nomeCampo;
            OLogAlteracao.justificativa = justificativa;
            OLogAlteracao.nomeCampoDisplay = nomeCampoDisplay;
            OLogAlteracao.oldValueSelect = oldValueSelect;
            OLogAlteracao.newValueSelect = novoValor.isEmpty() ? null : newValueSelect;

            switch (nomeCampo) {

                default: ORetorno = this.alterarCampo(OTituloDespesa, OLogAlteracao); break;
            }

            if (ORetorno.flagError) {
                return ORetorno;
            }

            ORetorno.flagError = false;
            ORetorno.listaErros.Add("Registro alterado com sucesso");
            ORetorno.info = OTituloDespesa.id;

            return ORetorno;
        }

        /// <summary>
        /// Faz a alteração de qualquer campo informado
        /// </summary>
        private UtilRetorno alterarCampo(TituloDespesa OItem, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);

            var listaCamposEditaveis = new[] {
                "flagFixa", "valorTotal", "descricao", "idContaBancaria", "idCentroCusto", "idMacroConta", "idCategoria",
                "nroNotaFiscal", "nroContabil", "nroContrato", "codigoBoleto", "nroDocumento", "idTipoDespesa", "idModoPagamento", "idContaBancariaFavorecida",
                "documentoPessoaCredor","nomePessoaCredor","nroTelPrincipalCredor"
            };

            if (!listaCamposEditaveis.Contains(OLog.nomeCampoAlterado)) {
                return UtilRetorno.newInstance(true, "Campo informado não pode ser editado");
            }

            OLog.valorAntigo = OItem.alterarValorCampo(OLog.nomeCampoAlterado, OLog.valorNovo);

            if (OLog.valorAntigo == null) {
                ORetorno.listaErros.Add("O valor informado é inválido");
                return ORetorno;
            }

            var successSave = db.SaveChanges();

            if (successSave > 0) {
                OLog.valorNovo = OItem.getValorCampo(OLog.nomeCampoAlterado);
                OLog.oldValueSelect = OLog.valorAntigo.isEmpty() ? null : OLog.oldValueSelect;
                OLogAlteracaoBL.salvar(OLog);

                ORetorno.flagError = false;
                return ORetorno;
            }

            ORetorno.listaErros.Add("Não foi possível salvar o registro no banco.");
            return ORetorno;
        }
    }
}