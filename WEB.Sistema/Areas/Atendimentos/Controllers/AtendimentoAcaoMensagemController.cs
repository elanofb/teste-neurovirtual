using System.Web.Mvc;
using BLL.Atendimentos;
using WEB.Areas.Atendimentos.ViewModels;

namespace WEB.Areas.Atendimentos.Controllers {

	public class AtendimentoAcaoMensagemController : Controller {

        //Atributos
        private IAtendimentoAcaoBL _IAtendimentoAcaoBL;

		//Propriedades
		private IAtendimentoAcaoBL OAtendimentoAcaoBL => _IAtendimentoAcaoBL = _IAtendimentoAcaoBL ?? new AtendimentoAcaoBL();

        //
        [ActionName("modal-enviar-mensagem")]
        public ActionResult modalEnviarMensagem(int id, int idStatus) {

            var ViewModel = new AtendimentoAcaoMensagemForm();

            ViewModel.AtendimentoHistorico.idAtendimento = id;

            ViewModel.AtendimentoHistorico.idStatusAtendimento = idStatus;

            return View(ViewModel);

        }

        [HttpPost, ActionName("enviar-mensagem")]
        public ActionResult enviarMensagem(AtendimentoAcaoMensagemForm ViewModel) {

            if (!ModelState.IsValid) {

                return View("modal-enviar-mensagem", ViewModel);
            }

            this.OAtendimentoAcaoBL.enviarMensagem(ViewModel.AtendimentoHistorico);

            return Json(new { error = false, message = "A mensagem foi enviada com sucesso." }, JsonRequestBehavior.AllowGet);

        }


    }
}