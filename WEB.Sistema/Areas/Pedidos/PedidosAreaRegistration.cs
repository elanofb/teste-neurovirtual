using System.Web.Mvc;

namespace WEB.Areas.Pedidos {
	public class PedidosAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Pedidos";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"Pedidos_default",
				"Pedidos/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}