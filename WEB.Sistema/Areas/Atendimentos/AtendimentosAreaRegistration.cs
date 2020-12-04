using System.Web.Mvc;

namespace WEB.Areas.Atendimentos {

	public class AtendimentosAreaRegistration : AreaRegistration {

		public override string AreaName => "Atendimentos";

	    public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
                "Atendimentos_default",
                "Atendimentos/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);

		}

	}
}