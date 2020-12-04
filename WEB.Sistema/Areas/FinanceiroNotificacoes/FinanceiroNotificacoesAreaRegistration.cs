using System.Web.Mvc;

namespace WEB.Areas.FinanceiroNotificacoes {

	public class FinanceiroNotificacoesAreaRegistration : AreaRegistration {

		public override string AreaName => "FinanceiroNotificacoes";

	    public override void RegisterArea(AreaRegistrationContext context) {
            
			context.MapRoute(
                "FinanceiroNotificacoes_Default",
                "FinanceiroNotificacoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);

		}

	}
}