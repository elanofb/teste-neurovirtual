using BLL.Institucionais;
using DAL.Institucionais;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Institucionais.ViewModels;

namespace WEB.Areas.Institucionais.Controllers
{
    public class AssociacaoHistoriaController : Controller
    {
        //Atributos
        private IAssociacaoHistoriaBL _AssociacaoHistoriaBL;

        //Propriedades
        private IAssociacaoHistoriaBL OAssociacaoHistoriaBL => (this._AssociacaoHistoriaBL = this._AssociacaoHistoriaBL ?? new AssociacaoHistoriaBL());

        //Construtor
        public AssociacaoHistoriaController() {
        }

        //
        public ActionResult listar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
            }

            var lista = this.OAssociacaoHistoriaBL.listar(idOrganizacao, true).ToList();

            return View(lista);
        }

        //
        [HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            AssociacaoHistoriaForm ViewModel = new AssociacaoHistoriaForm {
                AssociacaoHistoria = this.OAssociacaoHistoriaBL.carregar(idOrganizacao) ?? new AssociacaoHistoria(),
            };

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(AssociacaoHistoriaForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            if (User.idOrganizacao() > 0) {
                ViewModel.AssociacaoHistoria.idOrganizacao = User.idOrganizacao();
            }

            this.OAssociacaoHistoriaBL.salvar(ViewModel.AssociacaoHistoria);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso."));

            return RedirectToAction("editar", new { ViewModel.AssociacaoHistoria.idOrganizacao });

        }
    }
}