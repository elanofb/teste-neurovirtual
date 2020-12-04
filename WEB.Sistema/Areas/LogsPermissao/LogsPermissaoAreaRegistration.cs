using System.Web.Mvc;

namespace WEB.Areas.LogsPermissao {
	public class LogsPermissaoAreaRegistration : AreaRegistration {

		public override string AreaName {
			get {
				return "LogsPermissao";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) {

			context.MapRoute(
                "LogsPermissao_default",
                "LogsPermissao/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional },
				namespaces: new[] { "WEB.Areas.LogsPermissao.Controllers" }
			);
		}
	}
}