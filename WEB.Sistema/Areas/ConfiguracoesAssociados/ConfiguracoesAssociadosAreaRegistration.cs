using System.Web.Mvc;

namespace WEB.Areas.ConfiguracoesAssociados {

	public class ConfiguracoesAssociadosAreaRegistration : AreaRegistration {
		
		public override string AreaName => "ConfiguracoesAssociados";

	    public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"ConfiguracoesAssociados_Default",
				"ConfiguracoesAssociados/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}