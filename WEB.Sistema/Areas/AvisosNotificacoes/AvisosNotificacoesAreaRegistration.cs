using System.Web.Mvc;

namespace WEB.Areas.AvisosNotificacoes {
	public class AvisosNotificacoesAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "AvisosNotificacoes";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"AvisosNotificacoes_default",
				"AvisosNotificacoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}