using System.Web.Mvc;

namespace WEB.Areas.Transacoes {
	public class TransacoesAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Transacoes";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
				"Transacoes_default",
				"Transacoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}