using System;
using System.Text;
using System.Web.Mvc;

namespace WEB.Helpers {

    public static class LinkHelper {

        //Gerar o Html do Link
        private static string gerarLink(string urlEdicao, string extraClasses, string contentLink, string title, bool flagBlank) {

            StringBuilder htm = new StringBuilder();

            string target = (flagBlank ? "target='_blank'" : "");
            htm.Append("<a href='" + urlEdicao + "' class='" + String.Concat("", extraClasses) + "' " + target + " data-toggle='tooltip' title='" + title + "'>");
            htm.Append(contentLink);
            htm.Append("</a>");

            return htm.ToString();
        }

        //Link para editar um registro
        public static MvcHtmlString linkEditar(this HtmlHelper helper, int idRegistro, string urlEdicao = "", string extraClasses = "", string extraTexto = "") {

            StringBuilder htm = new StringBuilder();

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            string actionPadrao = "editar";

            urlEdicao = String.IsNullOrEmpty(urlEdicao) ? urlHelper.Action(actionPadrao, new { id = idRegistro }) : urlEdicao;

            string textLink = "<i class=\"fa fa-edit\"></i>";

            if (!string.IsNullOrEmpty(extraTexto)) {

                textLink = string.Concat(textLink, " ", extraTexto);
            }

            htm.Append(gerarLink(urlEdicao, extraClasses, textLink, "Editar Registro", false));

            return new MvcHtmlString(htm.ToString());
        }

        //
        public static MvcHtmlString link(this HtmlHelper helper, string textoLink, string urlLink, string title = "", string extraClasses = "", bool flagBlank = false) {
            StringBuilder htm = new StringBuilder();

            htm.Append(gerarLink(urlLink, extraClasses, textoLink, title, flagBlank));

            return new MvcHtmlString(htm.ToString());
        }

        //
        public static MvcHtmlString linkTexto(this HtmlHelper helper, string textoLink, string urlLink, string extraClasses = "", bool flagBlank = false) {
            StringBuilder htm = new StringBuilder();

            htm.Append(gerarLink(urlLink, extraClasses, textoLink, "", flagBlank));

            return new MvcHtmlString(htm.ToString());
        }

    }
}