using System;
using System.Linq;
using BLL.Financeiro.Interface;
using BLL.LogsAlteracoes;
using BLL.Services;
using DAL.Entities;
using DAL.Financeiro;
using DAL.LogsAlteracoes;

namespace BLL.Financeiro.Services {

    public class TituloReceitaAlteracaoDadoBL : DefaultBL, ITituloReceitaAlteracaoDadoBL {


        private ILogAlteracaoBL _LogAlteracaoBL;
        private ILogAlteracaoBL OLogAlteracaoBL
                => _LogAlteracaoBL = _LogAlteracaoBL
                ?? new LogAlteracaoBL();


        public UtilRetorno  alterarCampo(int id, string nomeCampo, string novoValor, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "", string justificativa = "") {

            UtilRetorno ORetorno = new UtilRetorno();

            var OTituloReceita = db.TituloReceita.condicoesSeguranca().FirstOrDefault(x => x.id == id);

            if (OTituloReceita == null) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o registro");

                return ORetorno;
            }

            if (OTituloReceita.dtExclusao != null) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não é possível alterar um registro que excluído");

                return ORetorno;
            }

            var OLogAlteracao = new LogAlteracao();

            OLogAlteracao.idEntidadeReferencia = EntityTypesConst.EVENTO;
            OLogAlteracao.idReferencia = id;
            OLogAlteracao.valorNovo = novoValor;
            OLogAlteracao.nomeCampoAlterado = nomeCampo;
            OLogAlteracao.justificativa = justificativa.abreviar(100);
            OLogAlteracao.nomeCampoDisplay = nomeCampoDisplay;
            OLogAlteracao.oldValueSelect = oldValueSelect;
            OLogAlteracao.newValueSelect = novoValor.isEmpty() ? null : newValueSelect;

            ORetorno = this.alterarCampo(OTituloReceita, OLogAlteracao);

            if (ORetorno.flagError) {
                return ORetorno;
            }

            ORetorno.flagError = false;
            ORetorno.listaErros.Add("Registro alterado com sucesso");
            ORetorno.info = OTituloReceita.id.ToString();

            return ORetorno;
        }

        private UtilRetorno alterarCampo(TituloReceita OTituloReceita, LogAlteracao OLog) {

            var ORetorno = UtilRetorno.newInstance(true);

            var listaCamposNaoEditaveis = new[] {
                  "id"
                , "idOrganizacao"
                , "idUsuarioCadastro"
                , "idUsuarioAlteracao"
                , "dtCadastro"
            };

            if (listaCamposNaoEditaveis.Contains(OLog.nomeCampoAlterado)) {

                return UtilRetorno.newInstance(true, "O dado informado não pode ser alterado.");
            }

            if (OLog.nomeCampoAlterado.Equals("observacao")) {

                OLog.valorNovo = OLog.valorNovo.abreviar(1000);
            }

            OLog.valorAntigo = OTituloReceita.alterarValorCampo(OLog.nomeCampoAlterado, OLog.valorNovo);

            if (OLog.valorAntigo == null) {

                ORetorno.listaErros.Add("O valor informado é inválido");

                return ORetorno;
            }
            
            var Retorno = db.validateAndSave();

            if (Retorno.flagError){

                return Retorno;
            }

            OLog.nomeCampoAlterado = OLog.nomeCampoAlterado.abreviar(255);
            OLog.nomeCampoDisplay  = OLog.nomeCampoDisplay.abreviar(255);
            OLog.valorNovo         = OTituloReceita.getValorCampo(OLog.nomeCampoAlterado).removeTags().abreviar(255);
            OLog.valorAntigo       = OLog.valorAntigo.removeTags().abreviar(255);
            OLog.oldValueSelect    = OLog.valorAntigo.isEmpty() ? null : OLog.oldValueSelect.removeTags().abreviar(255);

            OLogAlteracaoBL.salvar(OLog);

            ORetorno.flagError = false;

            return ORetorno;
        }
    }
}
