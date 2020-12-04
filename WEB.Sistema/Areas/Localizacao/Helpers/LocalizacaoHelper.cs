using System;
using System.Web.Mvc;

namespace WEB.Helpers{
    public static class LocalizacaoHelper{

		//
		public static MvcHtmlString includeLocalizacaoJS(this HtmlHelper helper) {
			System.Text.StringBuilder htm = new System.Text.StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<script  type=\"text/javascript\" src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "Areas/Localizacao/js/localizacao.js?v=1.0")).Append("\"></script>");
			return new MvcHtmlString(htm.ToString());
		}
    }
}