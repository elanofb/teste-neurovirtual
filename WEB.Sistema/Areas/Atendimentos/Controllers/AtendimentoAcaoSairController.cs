using System.Web.Mvc;
using BLL.Atendimentos;
using WEB.Areas.Atendimentos.ViewModels;

namespace WEB.Areas.Atendimentos.Controllers {

	public class AtendimentoAcaoSairController : Controller {

        //Atributos
        private IAtendimentoAcaoBL _IAtendimentoAcaoBL;

		//Propriedades
		private IAtendimentoAcaoBL OAtendimentoAcaoBL => _IAtendimentoAcaoBL = _IAtendimentoAcaoBL ?? new AtendimentoAcaoBL();

        //
        [ActionName("modal-sair")]
        public ActionResult modalFinalizar(int id) {

            var ViewModel = new AtendimentoAcaoSairForm();

            ViewModel.AtendimentoHistorico.idAtendimento = id;

            return View(ViewModel);

        }

        [HttpPost, ActionName("sair")]
        public ActionResult finalizar(AtendimentoAcaoSairForm ViewModel) {

            if (!ModelState.IsValid) {

                return View("modal-sair", ViewModel);

            }

            this.OAtendimentoAcaoBL.sair(ViewModel.AtendimentoHistorico);

            var urlRedirect = Url.Action("em-andamento", "Atendimento");

            return Json(new { error = false, message = "Você deixou o atendimento com sucesso.", urlRedirect }, JsonRequestBehavior.AllowGet);

        }


    }
}