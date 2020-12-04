using System.Web.Mvc;

namespace WEB.Areas.Notificacoes {
	public class NotificacoesAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "Notificacoes";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"Notificacoes_default",
				"Notificacoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}