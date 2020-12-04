using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.Notificacoes.ViewModels;

namespace WEB.Areas.Notificacoes.Controllers {

	[OrganizacaoFilter]
	public class TemplateMensagemConsultaController: BaseSistemaController {

		//
		public ActionResult Index() {
            
			var ViewModel = new TemplateMensagemConsultaVM();

			ViewModel.carregarInformacoes();

			return View(ViewModel);
            
		}
		
		//
		[ActionName("modal-pre-visualizar")]
		public ActionResult PreVisualizar(int id) {
            
			var ViewModel = new TemplateMensagemForm();

			ViewModel.carregar(id);

			return View(ViewModel);
            
		}

	}
}