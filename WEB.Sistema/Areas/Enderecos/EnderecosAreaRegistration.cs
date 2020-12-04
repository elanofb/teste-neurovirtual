using System.Web.Mvc;

namespace WEB.Areas.Enderecos {
	public class EnderecosAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "Enderecos";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"Enderecos_default",
				"Enderecos/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}