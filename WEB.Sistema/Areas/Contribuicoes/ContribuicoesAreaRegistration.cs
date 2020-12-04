using System.Web.Mvc;

namespace WEB.Areas.Contribuicoes {
	public class ContribuicoesAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "Contribuicoes";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			 context.MapRoute(
				name: "Anuidade_contribuicao",
				url: "anuidade/{action}",
				defaults: new { controller = "Anuidade", AreaName = "Contribuicoes" },
				namespaces: new[] { "WEB.Areas.Contribuicoes.Controllers" }
			);

			 context.MapRoute(
				name: "Mensalidade_contribuicao",
				url: "mensalidade/{action}",
				defaults: new { controller = "Mensalidade", AreaName = "Contribuicoes" },
				namespaces: new[] { "WEB.Areas.Contribuicoes.Controllers" }
			);

			 context.MapRoute(
				name: "Contribuicoes_contribuicao",
				url: "contribuicao/{action}",
				defaults: new { controller = "contribuicao", AreaName = "Contribuicoes" },
				namespaces: new[] { "WEB.Areas.Contribuicoes.Controllers" }
			);

			context.MapRoute(
				"Contribuicoes_default",
				"Contribuicoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}