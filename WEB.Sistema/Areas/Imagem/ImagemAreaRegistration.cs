using System.Web.Mvc;

namespace WEB.Areas.Imagem {
	public class ImagemAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "Imagem";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"Imagem_default",
				"Imagem/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}