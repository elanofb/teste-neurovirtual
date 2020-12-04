using System.Web.Mvc;

namespace WEB.Areas.Cargos {
	public class CargosAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Cargos";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			 context.MapRoute(
				name: "Cargos_URL",
				url: "cargo/{action}",
				defaults: new { controller = "default", AreaName = "Cargos" },
				namespaces: new[] { "WEB.Areas.Cargos.Controllers" }
			);

			context.MapRoute(
				"Cargos_default",
				"Cargos/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}