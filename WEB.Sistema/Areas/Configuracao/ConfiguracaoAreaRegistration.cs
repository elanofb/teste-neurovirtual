using System.Web.Mvc;

namespace WEB.Areas.ConfiguracoesTextos {
	public class ConfiguracoesTextosAreaRegistration : AreaRegistration {
		
		public override string AreaName => "ConfiguracoesTextos";

	    public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
                "ConfiguracoesTextoso_default",
                "ConfiguracoesTextos/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}