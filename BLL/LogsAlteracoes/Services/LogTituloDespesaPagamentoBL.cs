using System;
using System.Linq;
using BLL.Services;
using DAL.Entities;
using DAL.Financeiro;
using DAL.LogsAlteracoes;

namespace BLL.LogsAlteracoes {

    public class LogTituloDespesaPagamentoBL : DefaultBL, ILogTituloDespesaPagamentoBL {

        //Atributo
        private ILogAlteracaoBL _LogAlteracaoBL;

        //Propriedade
        private ILogAlteracaoBL OLogAlteracaoBL => _LogAlteracaoBL = _LogAlteracaoBL ?? new LogAlteracaoBL();
        
        public UtilRetorno alterarCampo(int id, string nomeCampo, string novoValor, string justificativa, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "") {

            UtilRetorno ORetorno = new UtilRetorno();

            var OTituloDespesaPagamento = db.TituloDespesaPagamento.condicoesSeguranca().FirstOrDefault(x => x.id == id);
            if (OTituloDespesaPagamento == null) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o registro");
                return ORetorno;
            }

            if (OTituloDespesaPagamento.TituloDespesa.dtExclusao.HasValue || OTituloDespesaPagamento.dtExclusao.HasValue) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não é possível alterar um registro que esta excluído");
                return ORetorno;
            }

            var OLogAlteracao = new LogAlteracao();
            OLogAlteracao.idEntidadeReferencia = EntityTypesConst.TITULO_DESPESA_PAGAMENTO;
            OLogAlteracao.idReferencia = id;
            OLogAlteracao.valorNovo = novoValor;
            OLogAlteracao.nomeCampoAlterado = nomeCampo;
            OLogAlteracao.justificativa = justificativa;
            OLogAlteracao.nomeCampoDisplay = nomeCampoDisplay;
            OLogAlteracao.oldValueSelect = oldValueSelect;
            OLogAlteracao.newValueSelect = novoValor.isEmpty() ? null : newValueSelect;

            switch (nomeCampo) {

                case "dtCompetencia":  ORetorno = this.alterarCampoDtCompetencia(OTituloDespesaPagamento, OLogAlteracao); break;

                case "nroDocumento":  ORetorno = this.alterarCampoNroDocumento(OTituloDespesaPagamento, OLogAlteracao); break;

                case "valorOriginal":  ORetorno = this.alterarCampoValorOriginal(OTituloDespesaPagamento, OLogAlteracao); break;

                case "valorPago":  ORetorno = this.alterarCampoValorPago(OTituloDespesaPagamento, OLogAlteracao); break;

                default: ORetorno = this.alterarCampo(OTituloDespesaPagamento, OLogAlteracao); break;
            }

            if (ORetorno.flagError) {
                return ORetorno;
            }

            ORetorno.flagError = false;
            ORetorno.listaErros.Add("Registro alterado com sucesso");
            ORetorno.info = OTituloDespesaPagamento.idTituloDespesa;

            return ORetorno;
        }

        /// <summary>
        /// Faz a alteração de qualquer campo informado
        /// </summary>
        private UtilRetorno alterarCampo(TituloDespesaPagamento OItem, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);

            var listaCamposEditaveis = new[] {
                "idCentroCusto", "idMacroConta", "descParcela", "idCategoria", "idMeioPagamento", "idContaBancaria", "dtVencimento", "dtPrevisaoPagamento",
                "valorJuros", "valorMulta", "valorDesconto"
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
        private UtilRetorno alterarCampoValorOriginal(TituloDespesaPagamento OItem, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);

            OLog.valorAntigo = OItem.valorOriginal.ToString();

            var valorOrginal = UtilNumber.toDecimal(OLog.valorNovo);
            if (valorOrginal == 0) {
                ORetorno.listaErros.Add("O valor informado é inválido");
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
        /// Altera o valor pago do pagamento
        /// </summary>
        private UtilRetorno alterarCampoValorPago(TituloDespesaPagamento OItem, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);

            OLog.valorAntigo = OItem.valorPago.ToString();

            if (OItem.dtPagamento == null) {
                ORetorno.listaErros.Add("Não é possível alterar o valor pago de uma parcela sem pagamento registrado");
                return ORetorno;
            }

            var valorPago = UtilNumber.toDecimal(OLog.valorNovo);
            if (valorPago == 0) {
                ORetorno.listaErros.Add("O valor informado é inválido");
                return ORetorno;
            }

            OItem.valorPago = valorPago;
            var successSave = db.SaveChanges();

            if (successSave > 0) {

                OLog.valorNovo = OItem.valorPago.ToString();
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
        private UtilRetorno alterarCampoNroDocumento(TituloDespesaPagamento OItem, LogAlteracao OLog) {

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

        /// <summary>
        /// Altera Data de Competência
        /// </summary>
        private UtilRetorno alterarCampoDtCompetencia(TituloDespesaPagamento OItem, LogAlteracao OLog) {

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
    }
}