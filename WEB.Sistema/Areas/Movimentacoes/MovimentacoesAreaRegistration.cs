using System.Web.Mvc;

namespace WEB.Areas.Movimentacoes {
	public class MovimentacoesAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "Movimentacoes";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
				"Movimentacoes_default",
				"Movimentacoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}