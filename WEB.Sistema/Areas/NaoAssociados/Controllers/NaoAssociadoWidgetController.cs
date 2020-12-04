using System.Web.Mvc;
using WEB.Areas.NaoAssociados.ViewModels;

namespace WEB.Areas.NaoAssociados.Controllers {

	public class NaoAssociadoWidgetController : Controller {

		//Atributos

		//Propriedades
		
		//Construtor
		public NaoAssociadoWidgetController() { 
			
		}

		//Listagem dos associados do sistema
		[ActionName("widget-resumo-nao-associados")]
		public PartialViewResult widgetResumoNaoAssociados() {
            
			var ViewModel = new WidgetResumoNaoAssociado();

			ViewModel.carregarDados();

			return PartialView(ViewModel);
		}

	}
}