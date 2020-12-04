using System.Text;
using System.Web.Mvc;
using DAL.Financeiro;
using WEB.Areas.Recibos.Helpers;

namespace WEB.Areas.Financeiro.Helpers {

	public static class TituloReceitaPagamentoHelper {


	    public static MvcHtmlString linkRegistrarPagamento(this HtmlHelper helper, int idTituloReceitaPagamento, string textoLink, string cssClass = ""){

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("modal-registrar-pagamento", "ReceitaDetalhePagamentosOperacao", new { area = "Financeiro", id = idTituloReceitaPagamento});

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", "javascript:void(0);");

            htmlLink.Attributes.Add("onclick", "ReceitaRegistrarPagamento.modalRegistrarPagamento(this);");

            htmlLink.Attributes.Add("data-url", linkHref);

            htmlLink.Attributes.Add("data-id", idTituloReceitaPagamento.ToString());

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Registrar Pagamento");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());	        
	    }



        /// <summary>
        /// Link do detalhe da receita
        /// </summary>
	    public static MvcHtmlString linkDetalhes(this HtmlHelper helper, int idPagamento, string textoLink, string cssClass = ""){

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("editar", "ReceitaDetalhe", new { area = "Financeiro", id = idPagamento });

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", linkHref);

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Detalhes");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());	        
	    }

        /// <summary>
        /// Montagem do link para exclusao do registro de um titulo_receita_pagamento
        /// </summary>
	    public static MvcHtmlString linkExcluirRegistro(this HtmlHelper helper, int idPagamento, string textoLink, string cssClass = ""){

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("modal-excluir-receita-pagamento", "ReceitaDetalhePagamentosOperacao", new { area = "Financeiro", id = idPagamento});

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", "javascript:void(0);");

            htmlLink.Attributes.Add("onclick", "DefaultSistema.showModal('"+linkHref+"');");

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Excluir Pagamento");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());	            
	    }

        /// <summary>
        /// Gerar o menu de acoes para um titulo_receita_pagamento
        /// </summary>
        public static MvcHtmlString menuAcoes(this HtmlHelper helper, TituloReceitaPagamentoResumoVW OPagamento, bool flagIdDetalheTituloReceita = false) {

            StringBuilder html = new StringBuilder();

            html.AppendLine("<ul class=\"dropdown-menu dropdown-menu-right\">");

            html.AppendLine($"<li>{ helper.linkDetalhes((flagIdDetalheTituloReceita == true ? OPagamento.idTituloReceita : OPagamento.idTituloPagamento) ?? 0, "Detalhes Registro") }</li>");

            if (!OPagamento.dtPagamento.HasValue){

                html.AppendLine($"<li>{ helper.linkRegistrarPagamento(OPagamento.idTituloPagamento ?? 0, "Registrar Pagamento") }</li>");

            }
            
            if (OPagamento.dtPagamento.HasValue){

                html.AppendLine($"<li>{ helper.linkReciboTituloPagamento(OPagamento.idTituloPagamento ?? 0, "Recibo Pagamento") }</li>");

            }

            html.AppendLine("<li ole=\"separator\" class=\"divider\"></li>");

            html.AppendLine($"<li>{ helper.linkExcluirRegistro(OPagamento.idTituloPagamento ?? 0, "Excluir Registro") }</li>");

            html.AppendLine("</ul>");
    

        
            return new MvcHtmlString(html.ToString());
        }

	}
}