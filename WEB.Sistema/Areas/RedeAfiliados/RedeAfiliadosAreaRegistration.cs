using System.Web.Mvc;

namespace WEB.Areas.RedeAfiliados {
	
	public class RedeAfiliadosAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "RedeAfiliados";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
				"RedeAfiliados_default",
				"RedeAfiliados/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}