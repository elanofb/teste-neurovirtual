using System.Web.Mvc;

namespace WEB.Areas.RamosAtividade {
	public class RamosAtividadeRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "RamosAtividade";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			 context.MapRoute(
				name: "RamosAtividade_URL",
				url: "ramosatividade/{action}",
				defaults: new { controller = "default", AreaName = "Cargos" },
				namespaces: new[] { "WEB.Areas.Cargos.Controllers" }
			);

			context.MapRoute(
				"RamosAtividade_default",
				"ramosatividade/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}