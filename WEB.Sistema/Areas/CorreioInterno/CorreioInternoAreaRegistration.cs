using System.Web.Mvc;

namespace WEB.Areas.CorreioInterno {
	public class CorreioInternoAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "CorreioInterno";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"CorreioInterno_default",
				"CorreioInterno/{controller}/{action}/{id}",
				new { action = "Inicio", id = UrlParameter.Optional },
				namespaces: new[] { "WEB.Areas.CorreioInterno.Controllers" }
			);
		}
	}
}