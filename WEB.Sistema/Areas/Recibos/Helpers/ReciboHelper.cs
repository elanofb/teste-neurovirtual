using System;
using System.Web.Mvc;

namespace WEB.Areas.Recibos.Helpers {

    public static class ReciboHelper {

        /// <summary>
        /// Link do recibo de pagamento
        /// </summary>
	    public static MvcHtmlString linkReciboTituloPagamento(this HtmlHelper helper, int idPagamento, string textoLink, string cssClass = ""){

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("exibir-recibo", "Recibo", new { area = "Recibos", r = UtilCrypt.toBase64Encode(idPagamento) });

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", linkHref);

            htmlLink.Attributes.Add("target", "_blank");
            
            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Recibo Pagamento");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());	        
	    }

        /// <summary>
        /// Link do recibo de pagamento
        /// </summary>
	    public static MvcHtmlString linkReciboTitulo(this HtmlHelper helper, int idTituloReceita, string textoLink, string cssClass = ""){

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("exibir-recibo", "ReciboTitulo", new { area = "Recibos", t = UtilCrypt.toBase64Encode(idTituloReceita) });

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", linkHref);

            htmlLink.Attributes.Add("target", "_blank");
            
            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Recibo Pagamento");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());	        
	    }
    }
}