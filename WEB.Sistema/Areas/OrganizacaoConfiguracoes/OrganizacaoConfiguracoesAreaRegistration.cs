using System.Web.Mvc;

namespace WEB.Areas.OrganizacaoConfiguracoes {
	
	public class OrganizacaoConfiguracoesAreaRegistration : AreaRegistration {

		public override string AreaName => "OrganizacaoConfiguracoes";

		public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
				"OrganizacaoConfiguracoes_default",
				"OrganizacaoConfiguracoes/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}