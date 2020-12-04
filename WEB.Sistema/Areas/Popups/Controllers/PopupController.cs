using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using BLL.Popups;
using DAL.Popups;
using MvcFlashMessages;
using WEB.Areas.Popups.ViewModels;

namespace WEB.Areas.Popups.Controllers {

    public class PopupController : Controller {

        //Atributos
        private IHomePopupBL _HomePopupBL;

        //Propriedades
        private IHomePopupBL OHomePopupBL => _HomePopupBL = _HomePopupBL ?? new HomePopupBL();

        //Construtor
        public PopupController() {

        }

        //
        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            int? idPortal = UtilRequest.getInt32("idPortal");
            bool? ativo = UtilRequest.getBool("flagAtivo");

            var lista = this.OHomePopupBL.listar(descricao, ativo, idPortal).OrderBy(x => x.id);
            return View(lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {
            var ViewModel = new PopupForm();

            var OHomePopup = this.OHomePopupBL.carregar(UtilNumber.toInt32(id)) ?? new HomePopup();

            ViewModel.OHomePopup = OHomePopup;

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(PopupForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }
            
            bool flagSucesso = this.OHomePopupBL.salvar(ViewModel.OHomePopup);

            if (flagSucesso) {
                 this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));
                return RedirectToAction("editar", new { id = ViewModel.OHomePopup.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OHomePopupBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.OHomePopupBL.excluir(id));
        }
    }
}