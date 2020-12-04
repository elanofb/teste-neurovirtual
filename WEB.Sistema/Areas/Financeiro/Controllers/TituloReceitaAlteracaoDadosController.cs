using System;
using System.Web.Mvc;
using BLL.Financeiro.Services;
using BLL.Financeiro.Interface;
using MvcFlashMessages;
using UTIL.UtilClasses;

namespace WEB.Areas.Financeiro.Controllers {

    public class TituloReceitaAlteracaoDadosController : Controller {

        private ITituloReceitaAlteracaoDadoBL Instance;

        private ITituloReceitaAlteracaoDadoBL OAlteracaoDadoBL =>
            Instance = Instance
                       ?? new TituloReceitaAlteracaoDadoBL();

        [ActionName("alterar-dados"), HttpPost, ValidateInput(false)]
        public ActionResult alterarDados(EditableItem ViewModel) {

            var Retorno = OAlteracaoDadoBL.alterarCampo(
                                                        ViewModel.pk.toInt()
                                                      , ViewModel.name
                                                      , ViewModel.value
                                                      , ViewModel.nomeCampoDisplay
                                                      , ViewModel.oldValue
                                                      , ViewModel.newValue);

            if (!ViewModel.viewName.isEmpty() && Retorno.flagError == true) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Algo não funcionou como esperado, Contate o suporte técnico"));

                return PartialView(ViewModel.viewName, ViewModel);
            }

            return Json(new {error = Retorno.flagError, message = string.Join("<br />", Retorno.listaErros), ViewModel.targetBox, ViewModel.value});
        }

        // GET: Eventos/EventoAlteracaoDados
        [ActionName("modal-alterar-texteditor"), HttpPost, ValidateInput(false)]
        public ActionResult modalAlterarTextEditor(EditableItem ViewModel) {

            ViewModel.viewName = "modal-alterar-texteditor";

            return PartialView(ViewModel);
        }
    }

}
