using System.Web.Mvc;

namespace WEB.Areas.AssociadosDependentes {

	public class AssociadosDependentesAreaRegistration : AreaRegistration {

		public override string AreaName => "AssociadosDependentes";

	    public override void RegisterArea(AreaRegistrationContext context) {
			 
			context.MapRoute(
				"AssociadosDependentes_Default",
				"AssociadosDependentes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);

		}
	}
}