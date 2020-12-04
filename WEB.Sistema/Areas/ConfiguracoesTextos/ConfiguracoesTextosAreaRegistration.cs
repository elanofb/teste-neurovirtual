using System.Web.Mvc;

namespace WEB.Areas.Configuracao {
	public class ConfiguracaoAreaRegistration : AreaRegistration {
		
		public override string AreaName {
			get {
				return "Configuracao";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"Configuracao_default",
				"Configuracao/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}