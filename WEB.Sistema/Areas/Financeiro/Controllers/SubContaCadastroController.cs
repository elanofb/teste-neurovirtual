using System;
using System.Web.Mvc;
using BLL.Financeiro;
using DAL.Financeiro;
using MvcFlashMessages;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {
    public class SubContaCadastroController : Controller {

        private ICategoriaTituloBL _CategoriaTituloBL;

        //Propriedades
        private ICategoriaTituloBL OCategoriaTituloBL => _CategoriaTituloBL = _CategoriaTituloBL ?? new CategoriaTituloBL();

        [HttpGet]
        public ActionResult editar(int? id) {

            CategoriaTituloForm ViewModel = new CategoriaTituloForm();
            CategoriaTitulo OCategoriaTitulo = this.OCategoriaTituloBL.carregar(UtilNumber.toInt32(id)) ?? new CategoriaTitulo();

            ViewModel.CategoriaTitulo = OCategoriaTitulo;

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult editar(CategoriaTituloForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OCategoriaTituloBL.salvar(ViewModel.CategoriaTitulo);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.CategoriaTitulo.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {

            var ORetorno = this.OCategoriaTituloBL.alterarStatus(id);

            return Json(ORetorno);
        }

        //
        [HttpGet, ActionName("modal-cadastrar-sub-conta")]
        public ActionResult modalCadastrarSubConta() {

            CategoriaTituloForm ViewModel = new CategoriaTituloForm();

            var id = UtilRequest.getInt32("id");

            ViewModel.CategoriaTitulo = OCategoriaTituloBL.carregar(id) ?? new CategoriaTitulo();

            if (ViewModel.CategoriaTitulo.id > 0) {
                ViewModel.group = UtilRequest.getString("group");
                ViewBag.modalId = UtilRequest.getString("modalId");

                return View(ViewModel);
            }

            ViewModel.group = UtilRequest.getString("group");
            var idMacroConta = UtilRequest.getInt32("idMacroConta");
            ViewBag.modalId = UtilRequest.getString("modalId");

            ViewModel.CategoriaTitulo = new CategoriaTitulo();
            ViewModel.CategoriaTitulo.idMacroConta = idMacroConta;
            ViewModel.CategoriaTitulo.descricao = UtilRequest.getString("descricao");

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("salvar-sub-conta")]
        public ActionResult salvarMacroConta(CategoriaTituloForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-cadastrar-sub-conta", ViewModel);
            }

            bool flagSucesso = this.OCategoriaTituloBL.salvar(ViewModel.CategoriaTitulo);

            if (flagSucesso) {
                return Json(new { error = false, ViewModel.CategoriaTitulo.id, ViewModel.CategoriaTitulo.descricao, ViewModel.group });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View("modal-cadastrar-sub-conta", ViewModel);

        }
    }
}