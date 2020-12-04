using System.Web.Mvc;

namespace WEB.Areas.ConfiguracoesRedesSociais {

	public class ConfiguracoesRedesSociaisAreaRegistration : AreaRegistration {
		
		public override string AreaName => "ConfiguracoesRedesSociais";

	    public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"ConfiguracoesRedesSociais_Default",
				"ConfiguracoesRedesSociais/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}