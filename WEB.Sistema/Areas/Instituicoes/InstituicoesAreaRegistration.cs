using System.Web.Mvc;

namespace WEB.Areas.Instituicoes {
	public class InstituicoesRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Instituicoes";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

            context.MapRoute(
				name: "Instituicoes_instituicao",
				url: "instituicao/{action}",
				defaults: new { controller = "instituicao", AreaName = "Instituicoes" },
				namespaces: new[] { "WEB.Areas.Instituicoes.Controllers" }
			);

			context.MapRoute(
				"Instituicoes_default",
				"Instituicoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}