using System.Web.Mvc;

namespace WEB.Areas.MeiosDivulgacao {

	public class MeiosDivulgacaoRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "MeiosDivulgacao";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			 context.MapRoute(
				name: "MeiosDivulgacao_URL",
				url: "MeiosDivulgacao/{action}",
				defaults: new { controller = "default", AreaName = "Cargos" },
				namespaces: new[] { "WEB.Areas.MeiosDivulgacao.Controllers" }
			);

			context.MapRoute(
				"MeiosDivulgacao_default",
				"MeiosDivulgacao/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}