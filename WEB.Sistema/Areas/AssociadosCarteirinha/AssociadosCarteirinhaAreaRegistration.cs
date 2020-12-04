using System.Web.Mvc;

namespace WEB.Areas.AssociadosCarteirinha {

	public class AssociadosCarteirinhaAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "AssociadosCarteirinha";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"AssociadosCarteirinha_default",
				"AssociadosCarteirinha/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}