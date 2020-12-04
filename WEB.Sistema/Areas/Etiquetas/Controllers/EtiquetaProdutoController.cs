using System;
using System.Linq;
using System.Web.Mvc;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.Etiquetas.ViewModels;

namespace WEB.Areas.Etiquetas.Controllers {

	public class EtiquetaProdutoController : BaseSistemaController {

		public ActionResult Index(EtiquetaProdutoVM ViewModel) {
			
			ViewModel.carregarInformacoes();

			if (!ViewModel.listaProdutos.Any()) {
				
				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível localizar nenhum produto.");
			}

			if (ViewModel.OConfiguracaoEtiqueta == null) {
				
				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não há configurações de etiquetas.");
			}

			return View(ViewModel);
		}

	}
	
}