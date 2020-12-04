using System.Web.Mvc;

namespace WEB.Areas.NaoAssociados {
	public class NaoAssociadosAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "NaoAssociados";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			 context.MapRoute(
				name: "NaoAssociados_URL",
				url: "naoassociado/{action}",
				defaults: new { controller = "default", action = "index", AreaName = "Mailings" },
				namespaces: new[] { "WEB.Areas.NaoAssociados.Controllers" }
			);

			context.MapRoute(
				"NaoAssociados_default",
				"NaoAssociados/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}