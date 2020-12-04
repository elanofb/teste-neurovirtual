using System.Web.Mvc;

namespace WEB.Areas.ConfiguracoesCarteirinha {

	public class ConfiguracoesCarteirinhaAreaRegistration : AreaRegistration {
		
		public override string AreaName => "ConfiguracoesCarteirinha";

	    public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"ConfiguracoesCarteirinha_Default",
				"ConfiguracoesCarteirinha/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}