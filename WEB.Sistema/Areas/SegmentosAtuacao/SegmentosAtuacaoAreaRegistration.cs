using System.Web.Mvc;

namespace WEB.Areas.SegmentosAtuacao {
	public class SegmentosAtuacaoAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "SegmentosAtuacao";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {
			 context.MapRoute(
				name: "SegmentosAtuacao_URL",
				url: "SegmentoAtuacao/{action}/{id}",
				defaults: new { controller = "default", AreaName = "Empresas" },
				namespaces: new[] { "WEB.Areas.SegmentosAtuacao.Controllers" }
			);

			context.MapRoute(
                "SegmentosAtuacao_default",
                "SegmentosAtuacao/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}

	}
}