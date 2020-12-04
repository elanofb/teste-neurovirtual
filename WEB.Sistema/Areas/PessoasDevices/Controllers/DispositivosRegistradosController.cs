using System.Linq;
using System.Web.Mvc;
using WEB.Areas.PessoasDevices.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.PessoasDevices.Controllers {

	public class DispositivosRegistradosController : Controller {

	    //
		public ActionResult Index() {

			var ViewModel = new DispositivosRegistradosVM();
			
			ViewModel.carregarInformacoes();
			
			if (ViewModel.flagTipoSaida == TipoSaidaHelper.EXCEL) {

				var OGerador = new GeradorCsvDispositivosRegistrados();

				var query = ViewModel.montarQuery();
                
				OGerador.baixarExcel(query.OrderByDescending(x => x.id).ToList());

				return null;
			}
			
			return View(ViewModel);
		}

	}
	
}
