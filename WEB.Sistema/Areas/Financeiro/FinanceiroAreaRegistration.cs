using System.Web.Mvc;

namespace WEB.Areas.Financeiro {
	public class FinanceiroAreaRegistration : AreaRegistration {
		public override string AreaName {
			get {
				return "financeiro";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			context.MapRoute(
				"financeiro_default",
				"financeiro/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}