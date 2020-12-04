using System.Web.Mvc;

namespace WEB.Areas.Etiquetas {

	public class EtiquetasAreaRegistration : AreaRegistration {

		public override string AreaName => "Etiquetas";

		public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
				"Etiquetas_default",
				"Etiquetas/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}