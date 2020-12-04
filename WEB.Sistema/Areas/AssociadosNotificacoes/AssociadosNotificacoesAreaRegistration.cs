using System.Web.Mvc;

namespace WEB.Areas.AssociadosNotificacoes {

	public class AssociadosNotificacoesAreaRegistration : AreaRegistration {

		public override string AreaName => "AssociadosNotificacoes";

	    public override void RegisterArea(AreaRegistrationContext context) {
            
			context.MapRoute(
				"AssociadosNotificacoes_Default",
				"AssociadosNotificacoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);

		}

	}
}