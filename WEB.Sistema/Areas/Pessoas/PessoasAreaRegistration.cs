using System.Web.Mvc;

namespace WEB.Areas.Pessoas {
	public class PessoasAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "Pessoas";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			
            context.MapRoute(
				name: "Pessoas_PessoaRelacionamento",
				url: "pessoarelacionamento/{action}",
				defaults: new { controller = "pessoarelacionamento", AreaName = "Pessoas" },
				namespaces: new[] { "WEB.Areas.Pessoas.Controllers" }
			);

            context.MapRoute(
				"Pessoas_default",
				"Pessoas/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}