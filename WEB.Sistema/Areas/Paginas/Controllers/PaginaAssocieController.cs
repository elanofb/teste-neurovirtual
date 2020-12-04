using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Paginas;
using DAL.Paginas;
using WEB.Areas.Paginas.ViewModels;

namespace WEB.Areas.Paginas.Controllers {

    public class PaginaAssocieController : Controller {

        //Atributos
        private IPaginaAssocieBL _IPaginaAssocieBL;

        //Propriedades
        private IPaginaAssocieBL OPaginaAssocieBL => _IPaginaAssocieBL = _IPaginaAssocieBL ?? new PaginaAssocieBL();

        //Construtor
        public PaginaAssocieController() {

        }

        //
        public ActionResult listar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if(User.idOrganizacao() > 0) {
                return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
            }

            var lista = this.OPaginaAssocieBL.listar(idOrganizacao).ToList();

            return View(lista);
        }

        //
        [HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if(User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            var ViewModel = new PaginaAssocieForm {
                PaginaAssocie = this.OPaginaAssocieBL.carregar(idOrganizacao) ?? new PaginaAssocie(),
            };

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(PaginaAssocieForm ViewModel) {

            if(!ModelState.IsValid) {
                return View(ViewModel);
            }

            if(User.idOrganizacao() > 0) {
                ViewModel.PaginaAssocie.idOrganizacao = User.idOrganizacao();
            }

            this.OPaginaAssocieBL.salvar(ViewModel.PaginaAssocie);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "Os dados da página foram salvos com sucesso."));

            return RedirectToAction("editar", new { ViewModel.PaginaAssocie.idOrganizacao });

        }
    }
}