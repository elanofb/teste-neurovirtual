using WEB.App_Infrastructure;

namespace System.Web.Mvc.Html {

	public static class HtmlSecurity {

		//
		public static bool temPermissao(this HtmlHelper helper, int idPerfilAcesso, string actionName, string controllerName, string areaName) {

		    return SecurityConfig.getInstance.verificarAutorizacao(idPerfilAcesso, actionName, controllerName, areaName);
		}

	}
}