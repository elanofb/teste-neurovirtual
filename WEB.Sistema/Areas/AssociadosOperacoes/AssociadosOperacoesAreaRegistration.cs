using System.Web.Mvc;

namespace WEB.Areas.AssociadosOperacoes {

	public class AssociadosOperacoesRegistration : AreaRegistration {

		public override string AreaName => "AssociadosOperacoes";

	    public override void RegisterArea(AreaRegistrationContext context) {
			 
			context.MapRoute(
				"AssociadosOperacoes_default",
				"AssociadosOperacoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);

		}

	}
}