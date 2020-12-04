using System.Web.Mvc;

namespace WEB.Areas.MateriaisApoio {
	public class MateriasApoioAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "MateriaisApoio";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
				"materiaisapoio_default",
				"materiaisapoio/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}