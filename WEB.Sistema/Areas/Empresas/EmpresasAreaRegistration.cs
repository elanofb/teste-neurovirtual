using System.Web.Mvc;

namespace WEB.Areas.Empresas {
	public class EmpresasAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Empresas";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			 context.MapRoute(
				name: "Empresas_URL",
				url: "empresa/{action}/{id}",
				defaults: new { controller = "default", AreaName = "Empresas" },
				namespaces: new[] { "WEB.Areas.Empresas.Controllers" }
			);

			context.MapRoute(
				"Empresas_default",
				"Empresas/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}