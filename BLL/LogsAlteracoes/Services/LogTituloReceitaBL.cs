using System;
using System.Linq;
using BLL.Services;
using DAL.Entities;
using DAL.Financeiro;
using DAL.LogsAlteracoes;

namespace BLL.LogsAlteracoes {

    public class LogTituloReceitaBL : DefaultBL, ILogTituloReceitaBL {

        //Atributo
        private ILogAlteracaoBL _LogAlteracaoBL;

        //Propriedade
        private ILogAlteracaoBL OLogAlteracaoBL => _LogAlteracaoBL = _LogAlteracaoBL ?? new LogAlteracaoBL();

        
        public UtilRetorno alterarCampo(int id, string nomeCampo, string novoValor, string justificativa, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "") {

            UtilRetorno ORetorno = new UtilRetorno();

            var OTituloReceita = db.TituloReceita.condicoesSeguranca().FirstOrDefault(x => x.id == id);

            if (OTituloReceita == null) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o registro");
                return ORetorno;
            }

            if (OTituloReceita.dtExclusao.HasValue) {
                return UtilRetorno.newInstance(true, "Não é possível alterar um registro que esta excluído");
            }

            var OLogAlteracao = new LogAlteracao();
            OLogAlteracao.idEntidadeReferencia = EntityTypesConst.TITULO_RECEITA;
            OLogAlteracao.idReferencia = id;
            OLogAlteracao.valorNovo = novoValor;
            OLogAlteracao.nomeCampoAlterado = nomeCampo;
            OLogAlteracao.justificativa = justificativa;
            OLogAlteracao.nomeCampoDisplay = nomeCampoDisplay;
            OLogAlteracao.oldValueSelect = oldValueSelect;
            OLogAlteracao.newValueSelect = novoValor.isEmpty() ? null : newValueSelect;

            switch (nomeCampo){

                default: ORetorno = this.alterarCampo(OTituloReceita, OLogAlteracao); break;
            }
            if (ORetorno.flagError) {
                return ORetorno;
            }

            ORetorno.flagError = false;
            ORetorno.listaErros.Add("Registro alterado com sucesso");
            ORetorno.info = OTituloReceita.id;

            return ORetorno;
        }

        /// <summary>
        /// Faz a alteração de qualquer campo informado
        /// </summary>
        private UtilRetorno alterarCampo(TituloReceita OItem, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);

            var listaCamposEditaveis = new[] {
                "observacao", "flagFixa", "valorTotal", "idContaBancaria", "idCentroCusto", "idMacroConta", "idCategoria", "idGatewayPermitido", "limiteParcelamento",
                "nroNotaFiscal", "nroContabil", "nroContrato", "nroDocumento", "dtLimitePagamento", 
                "flagBoletoBancarioPermitido", "flagDepositoPermitido", "flagCartaoCreditoPermitido", "emailPrincipal", "descricao"
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