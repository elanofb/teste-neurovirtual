using System.Web.Mvc;
using BLL.Atendimentos;
using WEB.Areas.Atendimentos.ViewModels;

namespace WEB.Areas.Atendimentos.Controllers {

	public class AtendimentoAcaoAguardarRetornoController : Controller {

        //Atributos
        private IAtendimentoAcaoBL _IAtendimentoAcaoBL;

		//Propriedades
		private IAtendimentoAcaoBL OAtendimentoAcaoBL => _IAtendimentoAcaoBL = _IAtendimentoAcaoBL ?? new AtendimentoAcaoBL();

        //
        [ActionName("modal-aguardar-retorno")]
        public ActionResult modalAguardarRetorno(int id) {

            var ViewModel = new AtendimentoAcaoMensagemForm();

            ViewModel.AtendimentoHistorico.idAtendimento = id;

            return View(ViewModel);

        }

        [HttpPost, ActionName("registrar-mensagem")]
        public ActionResult registrarMensagem(AtendimentoAcaoMensagemForm ViewModel) {

            if (!ModelState.IsValid) {

                return View("modal-aguardar-retorno", ViewModel);

            }

            this.OAtendimentoAcaoBL.aguardarRetorno(ViewModel.AtendimentoHistorico);

            var urlRedirect = Url.Action("em-andamento", "Atendimento");

            return Json(new { error = false, message = "Sucesso. O atendimento agora está a espera de um retorno.", urlRedirect }, JsonRequestBehavior.AllowGet);

        }


    }
}