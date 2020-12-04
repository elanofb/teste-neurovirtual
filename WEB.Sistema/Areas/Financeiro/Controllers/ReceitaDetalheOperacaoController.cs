using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using BLL.LogsAlteracoes;
using DAL.Financeiro;
using MvcFlashMessages;
using WEB.Areas.Financeiro.Helpers;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
	public class ReceitaDetalheOperacaoController : Controller {

        //Atributos
        private TituloReceitaPadraoBL _TituloReceitaPadraoBL;
        private ITituloReceitaExclusaoBL _TituloReceitaExclusaoBL;
	    private ILogTituloReceitaBL _LogTituloReceitaBL;

        //Propriedades
        private TituloReceitaPadraoBL OTituloReceitaBL => this._TituloReceitaPadraoBL = this._TituloReceitaPadraoBL ?? new TituloReceitaPadraoBL();
        private ITituloReceitaExclusaoBL OTituloReceitaExclusaoBL => this._TituloReceitaExclusaoBL = this._TituloReceitaExclusaoBL ?? new TituloReceitaExclusaoBL();
        private ILogTituloReceitaBL OLogTituloReceitaBL => _LogTituloReceitaBL = _LogTituloReceitaBL ?? new LogTituloReceitaBL();

        //Recebe as alterações do titulo de cobrança
        //Os valores são enviados individualmente
        [HttpPost, ActionName("alterar-dados")]
        public ActionResult alterarDados() {

            var id = UtilRequest.getInt32("pk");
            var nomeCampo = UtilRequest.getString("name").Trim();
            var valorCampo = UtilRequest.getString("value").Trim();
            var nomeCampoDisplay = UtilRequest.getString("nomeCampoDisplay").Trim();
            var oldValue = UtilRequest.getString("oldValue").Trim();
            var newValue = UtilRequest.getString("newValue").Trim();

            var ORetorno = OLogTituloReceitaBL.alterarCampo(
                id
                , nomeCampo
                , valorCampo
                , ""
                , nomeCampoDisplay
                , oldValue
                , newValue
            );

            return Json(ORetorno);
        }

        [HttpGet, ActionName("modal-excluir-receita")]
        public ActionResult modalExcluirReceita(int? id) {

            var OTituloReceita = this.OTituloReceitaBL.carregar(id.toInt());

            if (OTituloReceita == null) {
                return Json(new {flagErro = true, message = "A receita não pode ser localizada."}, JsonRequestBehavior.AllowGet);
            }

            if (FinanceiroHelper.receitasBloqueadas(OTituloReceita.idTipoReceita)){
                return Json(new { flagErro = true, message = "A receita não pode ser removida por esse modulo." }, JsonRequestBehavior.AllowGet);
            }

            if (OTituloReceita.dtQuitacao.HasValue) {
                return Json(new { flagErro = true, message = "Não é possível excluir uma receita quitada." }, JsonRequestBehavior.AllowGet);
            }

            if (OTituloReceita.listaTituloReceitaPagamento.Any(x => x.dtPagamento.HasValue)) {
                return Json(new { flagErro = true, message = "Não é possível excluir uma receita com baixa em algum pagamento." }, JsonRequestBehavior.AllowGet);
            }

            return View(OTituloReceita);
        }


        [HttpPost]
        public ActionResult excluir(TituloReceita ViewModel) {

            if (ViewModel.motivoExclusao.isEmpty()) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Informe o motivo da exclusão.");
                return View("modal-excluir-receita", ViewModel);
            }
            
            var ORetorno = this.OTituloReceitaExclusaoBL.excluir(ViewModel.id, ViewModel.motivoExclusao);

            if (ORetorno.flagError == false) {
                return Json(new {error = false, message = "Receita removida com sucesso", urlRetorno = Url.Action("editar", "ReceitaDetalhe", new {area = "Financeiro", id = ViewModel.id })});
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, ORetorno.listaErros.FirstOrDefault());
            return View("modal-excluir-receita", ViewModel);
        }
    }
}