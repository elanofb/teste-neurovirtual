using System.Web.Mvc;

namespace WEB.Areas.Representantes {
	public class RepresentantesAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Representantes";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			 context.MapRoute(
				name: "Representantes_URL",
				url: "Representante/{action}/{id}",
				defaults: new { controller = "default", AreaName = "Empresas" },
				namespaces: new[] { "WEB.Areas.Representantes.Controllers" }
			);

			context.MapRoute(
                "Representantes_default",
                "Representantes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}