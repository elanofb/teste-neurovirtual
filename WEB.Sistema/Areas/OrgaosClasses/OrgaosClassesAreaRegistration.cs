using System.Web.Mvc;

namespace WEB.Areas.OrgaosClasses {
	public class OrgaosClassesRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "OrgaosClasses";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
				"OrgaosClasses_default",
				"OrgaosClasses/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}