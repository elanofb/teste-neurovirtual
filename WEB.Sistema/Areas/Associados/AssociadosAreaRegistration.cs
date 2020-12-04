using System.Web.Mvc;

namespace WEB.Areas.Associados {

	public class AssociadosAreaRegistration : AreaRegistration {

		public override string AreaName => "Associados";

	    public override void RegisterArea(AreaRegistrationContext context) {
			 context.MapRoute(
				name: "Associado_Foto",
				url: "associado-foto/{action}/{id}",
				defaults: new { controller = "AssociadoFoto", AreaName = "Associados" },
				namespaces: new[] { "WEB.Areas.Associados.Controllers" }
			);

			context.MapRoute(
				"Associados_default",
				"Associados/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}