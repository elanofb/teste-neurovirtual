using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Paginas;
using DAL.Paginas;
using WEB.Areas.Paginas.ViewModels;

namespace WEB.Areas.Paginas.Controllers {

    public class PaginaEstatutoController : Controller {

        //Atributos
        private IPaginaEstatutoBL _IPaginaEstatutoBL;

        //Propriedades
        private IPaginaEstatutoBL OPaginaEstatutoBL => _IPaginaEstatutoBL = _IPaginaEstatutoBL ?? new PaginaEstatutoBL();

        //Construtor
        public PaginaEstatutoController() {

        }

        //
        public ActionResult listar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if(User.idOrganizacao() > 0) {
                return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
            }

            var lista = this.OPaginaEstatutoBL.listar(idOrganizacao).ToList();

            return View(lista);
        }

        //
        [HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if(User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            var ViewModel = new PaginaEstatutoForm {
                PaginaEstatuto = this.OPaginaEstatutoBL.carregar(idOrganizacao) ?? new PaginaEstatuto(),
            };

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(PaginaEstatutoForm ViewModel) {

            if(!ModelState.IsValid) {
                return View(ViewModel);
            }

            if(User.idOrganizacao() > 0) {
                ViewModel.PaginaEstatuto.idOrganizacao = User.idOrganizacao();
            }

            this.OPaginaEstatutoBL.salvar(ViewModel.PaginaEstatuto);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "Os dados da página foram salvos com sucesso."));

            return RedirectToAction("editar", new { ViewModel.PaginaEstatuto.idOrganizacao });

        }
    }
}