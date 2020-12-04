using System;
using System.Linq;
using BLL.Services;
using DAL.Entities;
using DAL.Financeiro;
using DAL.LogsAlteracoes;

namespace BLL.LogsAlteracoes {

    public class LogTituloReceitaPagamentoBL : DefaultBL, ILogTituloReceitaPagamentoBL{

        //Atributo
        private ILogAlteracaoBL _LogAlteracaoBL;

        //Propriedade
        private ILogAlteracaoBL OLogAlteracaoBL => _LogAlteracaoBL = _LogAlteracaoBL ?? new LogAlteracaoBL();
        
        public UtilRetorno alterarCampo(int id, string nomeCampo, string novoValor, string justificativa, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "") {

            UtilRetorno ORetorno = new UtilRetorno();

            var OTituloReceitaPagamento = db.TituloReceitaPagamento.condicoesSeguranca().FirstOrDefault(x => x.id == id);
            if (OTituloReceitaPagamento == null) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o registro");
                return ORetorno;
            }

            if (OTituloReceitaPagamento.TituloReceita.dtExclusao.HasValue || OTituloReceitaPagamento.dtExclusao.HasValue) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não é possível alterar um registro que esta excluído");
                return ORetorno;
            }

            var OLogAlteracao = new LogAlteracao();
            OLogAlteracao.idEntidadeReferencia = EntityTypesConst.TITULO_RECEITA_PAGAMENTO;
            OLogAlteracao.idReferencia = id;
            OLogAlteracao.valorNovo = novoValor;
            OLogAlteracao.nomeCampoAlterado = nomeCampo;
            OLogAlteracao.justificativa = justificativa;
            OLogAlteracao.nomeCampoDisplay = nomeCampoDisplay;
            OLogAlteracao.oldValueSelect = oldValueSelect;
            OLogAlteracao.newValueSelect = novoValor.isEmpty() ? null : newValueSelect;

            switch (nomeCampo) {

                case "dtCompetencia":  ORetorno = this.alterarCampoDtCompetencia(OTituloReceitaPagamento, OLogAlteracao); break;

                case "nroDocumento":  ORetorno = this.alterarCampoNroDocumento(OTituloReceitaPagamento, OLogAlteracao); break;

                case "valorOriginal":  ORetorno = this.alterarCampoValorOriginal(OTituloReceitaPagamento, OLogAlteracao); break;

                case "valorRecebido":  ORetorno = this.alterarCampoValorRecebido(OTituloReceitaPagamento, OLogAlteracao); break;

                default: ORetorno = this.alterarCampo(OTituloReceitaPagamento, OLogAlteracao); break;
            }

            if (ORetorno.flagError) {
                return ORetorno;
            }

            ORetorno.flagError = false;
            ORetorno.listaErros.Add("Registro alterado com sucesso");
            ORetorno.info = OTituloReceitaPagamento.idTituloReceita;

            return ORetorno;
        }


                /// <summary>
        /// Faz a alteração de qualquer campo informado
        /// </summary>
        private UtilRetorno alterarCampo(TituloReceitaPagamento OItem, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);

            var listaCamposEditaveis = new[] {
                "idContaBancaria", "idCentroCusto", "idMacroConta", "idCategoria", "idMeioPagamento", "dtVencimento", "dtPrevisaoCredito", "valorOutrasTarifas", "descricaoParcela"
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

        /// <summary>
        /// Altera o valor original do pagamento
        /// </summary>
        private UtilRetorno alterarCampoValorOriginal(TituloReceitaPagamento OItem, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);

            OLog.valorAntigo = OItem.valorOriginal.ToString();

            var valorOrginal = UtilNumber.toDecimal(OLog.valorNovo);
            if (valorOrginal == 0) {
                ORetorno.listaErros.Add("O valor informado é inválido");
                return ORetorno;
            }

            if (valorOrginal < OItem.valorTotalTarifas()) {
                ORetorno.listaErros.Add("O valor informado é menor que o total de tarifas");
                return ORetorno;
            }

            OItem.valorOriginal = valorOrginal;
            var successSave = db.SaveChanges();

            if (successSave > 0) {

                OLog.valorNovo = OItem.valorOriginal.ToString();
                OLog.oldValueSelect = OLog.valorAntigo.isEmpty() ? null : OLog.oldValueSelect;
                OLogAlteracaoBL.salvar(OLog);

                ORetorno.flagError = false;
                return ORetorno;
            }

            ORetorno.listaErros.Add("Não foi possível salvar o registro no banco.");
            return ORetorno;
        }

        
        /// <summary>
        /// Altera Data de Competência
        /// </summary>
        private UtilRetorno alterarCampoDtCompetencia(TituloReceitaPagamento OItem, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);
            
            OLog.valorAntigo = OItem.alterarValorCampo(OLog.nomeCampoAlterado, OLog.valorNovo);

            if (OLog.valorAntigo == null){
                ORetorno.listaErros.Add("O valor informado é inválido");
                return ORetorno;
            }

            OItem.mesCompetencia = Convert.ToByte(OItem.dtCompetencia?.Month);
            OItem.anoCompetencia = Convert.ToInt16(OItem.dtCompetencia?.Year);

            var successSave = db.SaveChanges();

            if (successSave > 0) {
                OLog.valorNovo = OItem.dtCompetencia.ToString();
                OLog.oldValueSelect = OLog.valorAntigo.isEmpty() ? null : OLog.oldValueSelect;
                OLogAlteracaoBL.salvar(OLog);

                ORetorno.flagError = false;
                return ORetorno;
            }

            ORetorno.listaErros.Add("Não foi possível salvar o registro no banco.");
            return ORetorno;
        }


        /// <summary>
        /// Altera o valor pago do pagamento
        /// </summary>
        private UtilRetorno alterarCampoValorRecebido(TituloReceitaPagamento OItem, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);

            OLog.valorAntigo = OItem.valorRecebido.ToString();

            if (OItem.dtPagamento == null) {
                ORetorno.listaErros.Add("Não é possível alterar o valor pago de uma parcela sem pagamento registrado");
                return ORetorno;
            }

            var valorRecebido = UtilNumber.toDecimal(OLog.valorNovo);
            if (valorRecebido == 0) {
                ORetorno.listaErros.Add("O valor informado é inválido");
                return ORetorno;
            }

            OItem.valorRecebido = valorRecebido;
            var successSave = db.SaveChanges();

            if (successSave > 0) {

                OLog.valorNovo = OItem.valorRecebido.ToString();
                OLog.oldValueSelect = OLog.valorAntigo.isEmpty() ? null : OLog.oldValueSelect;
                OLogAlteracaoBL.salvar(OLog);

                ORetorno.flagError = false;
                return ORetorno;
            }

            ORetorno.listaErros.Add("Não foi possível salvar o registro no banco.");
            return ORetorno;
        }
        
        /// <summary>
        /// Altera o nroDocumento do pagamento
        /// </summary>
        private UtilRetorno alterarCampoNroDocumento(TituloReceitaPagamento OItem, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);

            OLog.valorAntigo = OItem.nroDocumento;

            var nroDocumento = UtilString.onlyAlphaNumber(OLog.valorNovo);

            OItem.nroDocumento = nroDocumento;
            var successSave = db.SaveChanges();

            if (successSave > 0) {
                OLog.valorNovo = OItem.nroDocumento;
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