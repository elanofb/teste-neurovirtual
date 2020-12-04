using System.Web.Mvc;
using BLL.Notificacoes;
using WEB.App_Infrastructure;
using WEB.Areas.CorreioInterno.ViewModels;

namespace WEB.Areas.CorreioInterno.Controllers {

	//[RouteArea("CorreioInterno", AreaPrefix = "correio-interno")]
	public class ConfiguracaoContaController : BaseSistemaController {

		//Atributos
		private IConfiguracaoEmailUsuarioCadastroBL _ConfiguracaoEmailUsuarioCadastroBL;

		//Propriedades
		private IConfiguracaoEmailUsuarioCadastroBL OConfiguracaoEmailUsuarioCadastroBL => this._ConfiguracaoEmailUsuarioCadastroBL = this._ConfiguracaoEmailUsuarioCadastroBL ?? new ConfiguracaoEmailUsuarioCadastroBL();
		
		//
		public ActionResult index() { 
			return View();
		}

		//
		[ActionName("conta-email")]
		public ActionResult contaEmail(bool? flagBuscarEmails = true, string valorBusca = "") {

			var ViewModel = new ConfiguracaoContaForm();
			
			ViewModel.ConfiguracaoEmailUsuario = ConfiguracaoEmailUsuarioConsultaBL.getInstance.carregar();
			
			ViewModel.preencherAssinatura();

			ViewBag.actionPaginacao = "conta-email";
			
			return View(ViewModel);
		}
		
		//
		[ActionName("salvar-configuracao"), ValidateInput(false)]
		public ActionResult salvarConfiguracao(ConfiguracaoContaForm ViewModel) {

			if (!ModelState.IsValid) {
				return View("conta-email", ViewModel);
			}

			ViewModel.preencherAssinatura();
			
			var flagSucesso = OConfiguracaoEmailUsuarioCadastroBL.salvar(ViewModel.ConfiguracaoEmailUsuario);

			if (!flagSucesso) {
				
				return Json(new { error = true, message = "Erro ao salvar a configuração!" });
				
			}
			
			return Json(new { error = false, message = "Configuração salva com sucesso!" });
		}
		
	}
}
