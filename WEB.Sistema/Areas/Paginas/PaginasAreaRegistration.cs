using System.Web.Mvc;

namespace WEB.Areas.Paginas {

	public class PaginasAreaRegistration : AreaRegistration {

		public override string AreaName => "Paginas";

	    public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
				"Paginas_default",
				"Paginas/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);

		}

	}

}