using System.Web.Mvc;

namespace WEB.Areas.Mailings {
	public class MailingsAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Mailings";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			 context.MapRoute(
				name: "Mailings_URL",
				url: "mailing/{action}",
				defaults: new { controller = "default", action = "index", AreaName = "Mailings" },
				namespaces: new[] { "WEB.Areas.Mailings.Controllers" }
			);

			context.MapRoute(
				"Mailings_default",
				"Mailings/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}