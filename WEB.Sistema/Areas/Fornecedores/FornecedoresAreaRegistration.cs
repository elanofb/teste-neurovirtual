using System.Web.Mvc;

namespace WEB.Areas.Fornecedores {
	public class FornecedoresAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Fornecedores";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
				"Fornecedores_default",
				"Fornecedores/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
			
		}
	}
}