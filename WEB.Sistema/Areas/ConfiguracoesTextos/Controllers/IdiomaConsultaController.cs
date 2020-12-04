using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.ConfiguracoesTextos.ViewModels;

namespace WEB.Areas.ConfiguracoesTextos.Controllers {

	[OrganizacaoFilter]
    public class IdiomaConsultaController : BaseSistemaController {

        //
		[HttpGet]
        public ActionResult listar(){
		    
            var ViewModel = new IdiomaConsultaVM();
			
		    ViewModel.carregar();

            return View(ViewModel);
        }
    }
}