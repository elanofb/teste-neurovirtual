using System.Web.Mvc;

namespace WEB.Areas.Recibos {
	public class RecibosAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "Recibos";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"Recibos_default",
				"Recibos/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}