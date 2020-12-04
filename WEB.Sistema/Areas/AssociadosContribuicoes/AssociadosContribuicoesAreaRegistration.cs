using System.Web.Mvc;

namespace WEB.Areas.AssociadosContribuicoes {
	public class AssociadosContribuicoesAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "AssociadosContribuicoes";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			 context.MapRoute(
				name: "Associados_Contribuicoes",
				url: "associado-contribuicao/{action}/{id}",
				defaults: new { controller = "AssociadosContribuicoes", AreaName = "AssociadosContribuicoes" },
				namespaces: new[] { "WEB.Areas.AssociadosContribuicoes.Controllers" }
			);

			context.MapRoute(
				"AssociadosContribuicoes_default",
				"AssociadosContribuicoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}