using System.Web.Mvc;

namespace WEB.Areas.Erros {
	public class ErrosAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Erros";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"Erros_default",
				"Erros/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}