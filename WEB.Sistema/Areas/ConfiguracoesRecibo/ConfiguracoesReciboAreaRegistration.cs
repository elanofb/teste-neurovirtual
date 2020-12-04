using System.Web.Mvc;

namespace WEB.Areas.ConfiguracoesRecibo {

	public class ConfiguracoesReciboAreaRegistration : AreaRegistration {
		
		public override string AreaName => "ConfiguracoesRecibo";

	    public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"ConfiguracoesRecibo_Default",
				"ConfiguracoesRecibo/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}