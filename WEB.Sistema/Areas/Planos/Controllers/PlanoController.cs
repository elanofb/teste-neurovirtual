using System;
using BLL.Planos;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using WEB.Areas.Planos.ViewModels;
using DAL.Planos;
using MvcFlashMessages;

namespace WEB.Areas.Planos.Controllers {

    public class PlanoController : Controller {

        private IPlanoBL _PlanoBL;

        private IPlanoBL OPlanoBL => _PlanoBL = _PlanoBL ?? new PlanoBL(); 

        public PlanoController() {
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            string ativo = UtilRequest.getString("flagStatus");

            var listaPlano = this.OPlanoBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(listaPlano.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));

        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new PlanoForm();

            ViewModel.Plano = this.OPlanoBL.carregar(UtilNumber.toInt32(id)) ?? new Plano();

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(PlanoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OPlanoBL.salvar(ViewModel.Plano);

            if (flagSucesso) {
                 this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados do plano foram salvos com sucesso.");
                return View(ViewModel);
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o registro. Tente novamente.");
            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.OPlanoBL.excluir(id));
        }
    }
}