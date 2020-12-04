using System;
using System.Web.Mvc;

namespace WEB.Areas.Arquivos.Helpers{

    public static class ArquivoHelper{
       
        //INclusao de CSSs necessarios para modulo de Arquivos
        public static MvcHtmlString includeCSSModuloArquivos(this HtmlHelper helper) {
			System.Text.StringBuilder htm = new System.Text.StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "Areas/Arquivos/js/plugins/bootstrap-editable/bootstrap3-editable/css/bootstrap-editable.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "Areas/Arquivos/js/plugins/bootstrap-fileinput/css/fileinput.min.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "Areas/Arquivos/js/plugins/jqueryFancybox/jquery.fancybox.css")).Append("\" rel=\"stylesheet\" />");

			return new MvcHtmlString(htm.ToString());
		}

		//INclusao de JSs necessarios para modulo de Arquivos
		public static MvcHtmlString includeJSModuloArquivos(this HtmlHelper helper) {
			System.Text.StringBuilder htm = new System.Text.StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "Areas/Arquivos/js/plugins/bootstrap-editable/bootstrap3-editable/js/bootstrap-editable.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "Areas/Arquivos/js/plugins/bootstrap-fileinput/js/fileinput.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "Areas/Arquivos/js/plugins/jqueryFancybox/jquery.fancybox.js")).Append("\"></script>");
			return new MvcHtmlString(htm.ToString());
		}

        //INclusao de JSs necessarios para modulo de Arquivos
        public static MvcHtmlString includeJSModuloArquivosFotos(this HtmlHelper helper) {
            System.Text.StringBuilder htm = new System.Text.StringBuilder();
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/jQueryUI/jquery-ui.js")).Append("\"></script>");
            htm.Append(helper.includeJSModuloArquivos());
            htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "Areas/Arquivos/js/arquivo-foto.js")).Append("\"></script>");

            return new MvcHtmlString(htm.ToString());
        }

        //
        public static SelectList selectListTipoArquivo(string selected) {

            var list = new[] {
                    new{value = "img", text = "Imagem"},
                    new{value = "doc", text = "Documento"}
            };
            return new SelectList(list, "value", "text", selected);
        }

    }

}