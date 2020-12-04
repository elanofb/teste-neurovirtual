using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.Associados.Models.ViewModels;

namespace WEB.Areas.Associados.Controllers {

	[OrganizacaoFilter]
	public class PreAtualizacaoConsultaController : BaseSistemaController {
		
		//
		public ActionResult Index() {
			
			var ViewModel = new PreAtualizacaoConsultaVM();
			
			ViewModel.capturarParametros();
			
			ViewModel.carregarDados();
			
            return View(ViewModel);
			
		}

	}
	
}