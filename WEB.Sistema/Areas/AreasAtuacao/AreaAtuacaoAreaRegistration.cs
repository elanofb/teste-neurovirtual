using System.Web.Mvc;

namespace WEB.Areas.AreasAtuacao {

	public class AreaAtuacaosAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "AreasAtuacao";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			 context.MapRoute(
				name: "AreasAtuacao_URL",
				url: "AreaAtuacao/{action}",
				defaults: new { controller = "default", AreaName = "AreasAtuacao" },
				namespaces: new[] { "WEB.Areas.AreaAtuacao.Controllers" }
			);

			context.MapRoute(
				"AreasAtuacao_default",
				"AreasAtuacao/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}