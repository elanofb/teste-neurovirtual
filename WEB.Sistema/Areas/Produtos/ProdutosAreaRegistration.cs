using System.Web.Mvc;

namespace WEB.Areas.Produtos {
	public class ProdutosAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "Produtos";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
            
			context.MapRoute(
				"Produtos_default",
				"Produtos/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional },
				namespaces: new[] { "WEB.Areas.Produtos.Controllers" }
			);

		}
	}
}