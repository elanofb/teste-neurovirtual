using System.Web.Mvc;

namespace WEB.Areas.ConfiguracoesEcommerce {

	public class ConfiguracoesEcommerceAreaRegistration : AreaRegistration {
		
		public override string AreaName => "ConfiguracoesEcommerce";

	    public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"ConfiguracoesEcommerce_Default",
				"ConfiguracoesEcommerce/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}