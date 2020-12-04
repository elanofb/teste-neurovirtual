using System.Web.Mvc;

namespace WEB.Areas.Profissoes {
	public class ProfissoesAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Profissoes";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			 context.MapRoute(
				name: "Profissoes_URL",
				url: "profissao/{action}/{id}",
				defaults: new { controller = "default", AreaName = "Empresas" },
				namespaces: new[] { "WEB.Areas.Profissoes.Controllers" }
			);

			context.MapRoute(
                "Profissoes_default",
                "Profissoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}