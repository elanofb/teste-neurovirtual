using System.Web.Mvc;

namespace WEB.Areas.ConfiguracoesScripts {

	public class ConfiguracoesScriptsAreaRegistration : AreaRegistration {
		
		public override string AreaName => "ConfiguracoesScripts";

	    public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"ConfiguracoesScripts_Default",
				"ConfiguracoesScripts/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}