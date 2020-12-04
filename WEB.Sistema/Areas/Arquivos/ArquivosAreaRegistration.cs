using System.Web.Mvc;

namespace WEB.Areas.Arquivos {
	public class ArquivosRegistration : AreaRegistration {
		
		public override string AreaName => "Arquivos";

		public override void RegisterArea(AreaRegistrationContext context) {
			
			context.MapRoute(
				name: "Exibir_Arquivos",
				url: "Arquivo-Exibicao/{idNum}/{idCrypt}",
				defaults: new { action = "index", controller = "Exibicao", AreaName = "Arquivos", idNum = UrlParameter.Optional, idCrypt = UrlParameter.Optional },
				namespaces: new[] { "WEB.Areas.Arquivos.Controllers" }
			);
			
			context.MapRoute(
				"Arquivos_default",
				"Arquivos/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}