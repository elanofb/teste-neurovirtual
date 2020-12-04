using System.Web.Mvc;

namespace WEB.Areas.AssociadosOrganizacoes {

	public class AssociadosOrganizacoesAreaRegistration : AreaRegistration {

		public override string AreaName => "AssociadosOrganizacoes";

	    public override void RegisterArea(AreaRegistrationContext context) {
			 
		    context.MapRoute(
				"AssociadosOrganizacoes_Default",
				"AssociadosOrganizacoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		    
		}
	}
}