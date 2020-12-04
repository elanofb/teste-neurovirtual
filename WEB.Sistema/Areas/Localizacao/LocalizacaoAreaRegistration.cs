using System.Web.Mvc;

namespace WEB.Areas.Localizacao {
	public class LocalizacaoAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "Localizacao";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			 context.MapRoute(
				name: "Localizacao_Localizacao",
				url: "localizacao/{action}",
				defaults: new { controller = "default", AreaName = "localizacao" },
				namespaces: new[] { "WEB.Areas.Localizacao.Controllers" }
			);

			context.MapRoute(
				"Localizacao_default",
				"Localizacao/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}