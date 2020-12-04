using System;
using System.Text;
using System.Web.Mvc;
using BLL.Configuracoes;
using DAL.Financeiro;
using WEB.Areas.Recibos.Helpers;

namespace WEB.Areas.Financeiro.Helpers {

    public static class TituloReceitaExtensions {

        ///// <summary>
        ///// Gerar o menu de acoes para uma contribuica
        ///// </summary>
        public static MvcHtmlString menuAcoes(this HtmlHelper helper, TituloReceitaResumoVW OTituloReceita, bool flagLinkExclusao = true) {

            StringBuilder html = new StringBuilder();

            html.AppendLine("<button class=\"btn btn-default btn-white-sm dropdown-toggle\" data-toggle=\"dropdown\"> Ações <span class=\"caret\"></span></button>");

            html.AppendLine("<ul class=\"dropdown-menu dropdown-menu-right\">");

            if (!OTituloReceita.dtQuitacao.HasValue && OTituloReceita.qtdeParcelas <= 1) {

                html.AppendLine($"<li>{ helper.linkRegistrarPagamentoTitulo(OTituloReceita.id, "Registrar pagamento") }</li>");


                var ConfiguracaoContribuicao = ConfiguracaoContribuicaoBL.getInstance.carregar();

                if (ConfiguracaoContribuicao.limiteParcelamento.toByte() > 1) {

                    html.AppendLine($"<li>{ helper.linkParcelarTitulo(OTituloReceita.id, "Parcelar cobrança") }</li>");

                }
            }

            //if (OTituloReceita.qtdeParcelas > 1) {

            //    html.AppendLine($"<li>{ helper.linkDetalheParcelamento(OTituloReceita.id, "Detalhe parcelamento") }</li>");
            //}

            if (OTituloReceita.dtQuitacao.HasValue) {

                html.AppendLine($"<li>{ helper.linkReciboTitulo(OTituloReceita.id, "Recibo pagamento") }</li>");

            }

            if (flagLinkExclusao) {

                html.AppendLine("<li ole=\"separator\" class=\"divider\"></li>");

                html.AppendLine($"<li>{ helper.linkExcluirTitulo(OTituloReceita.id, "Excluir cobrança") }</li>");

            }

            html.AppendLine("</ul>");

            return new MvcHtmlString(html.ToString());
        }

        /// <summary>
        /// Montar link que deve abrir modal para registro de pagamento diretamente a partir de um título.
        /// </summary>
	    public static MvcHtmlString linkRegistrarPagamentoTitulo(this HtmlHelper helper, int idTituloReceita, string textoLink, string cssClass = "", string icon = "") {

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("modal-registrar-pagamento", "ReceitaBaixa", new { area = "Financeiro", id = idTituloReceita });

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", "javascript:void(0);");

            htmlLink.Attributes.Add("onclick", "ReceitaBaixa.modalRegistrarPagamento(this);");

            htmlLink.Attributes.Add("data-url", linkHref);

            htmlLink.Attributes.Add("data-id", idTituloReceita.ToString());

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Registrar Pagamento");

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            if (!icon.isEmpty()) {

                htmlLink.InnerHtml += $"<i class=\"{icon}\"></i> ";

            }

            htmlLink.InnerHtml += textoLink;

            string finalHtml = htmlLink.ToString();

            return new MvcHtmlString(finalHtml);
        }

        /// <summary>
        /// Link para abrir um modal para iniciar o processo de remoção de uma receita
        /// </summary>
        public static MvcHtmlString linkExcluirTitulo(this HtmlHelper helper, int idTituloReceita, string textoLink, string cssClass = "") {

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("modal-excluir-receita", "ReceitaDetalheOperacao", new { area = "Financeiro", id = idTituloReceita });

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", "javascript:void(0);");

            htmlLink.Attributes.Add("onclick", "DefaultSistema.showModal('" + linkHref + "')");

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Remover essa cobrança");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());
        }

        /// <summary>
        /// Gerar o link do checkout para um titulo_receita
        /// </summary>
        public static MvcHtmlString linkPagamentoTitulo(this HtmlHelper helper, int idTituloReceita, string textoLink, string cssClass = "", string icon = "") {


            var linkHref = UtilConfig.linkPgtoTitulo(idTituloReceita);

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", linkHref);

            htmlLink.Attributes.Add("target", "_blank");

            htmlLink.Attributes.Add("title", "Link de pagamento");

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            if (!icon.isEmpty()) {

                 htmlLink.InnerHtml += $"<i class=\"{icon}\"></i> ";

            }

            htmlLink.InnerHtml += textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());
        }

        /// <summary>
        /// Enviar cobrança de um titulo_receita por e-mail
        /// </summary>
        public static MvcHtmlString linkParcelarTitulo(this HtmlHelper helper, int idTituloReceita, string textoLink, string cssClass = "", string icon = "") {

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var url = urlHelper.Action("modal-parcelar-titulo", "TituloReceitaParcelamento", new { area="FinanceiroParcelamentos", id = idTituloReceita});

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", "javascript:void(0);");

            htmlLink.Attributes.Add("data-url", url);

            htmlLink.Attributes.Add("onclick", $"TituloReceitaParcelamento.abrirModalParcelamento(this)");

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Parcelamento a cobrança");

            if (!icon.isEmpty()) {

                htmlLink.InnerHtml = $"<i class=\"{icon}\"></i> ";

            }

            htmlLink.InnerHtml += textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());
        }

        /// <summary>
        /// Gerar o link para detalhamento do titulo de receita
        /// </summary>
        public static MvcHtmlString linkDetalheTitulo(this HtmlHelper helper, int idTituloReceita, string textoLink, string cssClass = "", string icon = "") {

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("editar", "ReceitaDetalhe", new { area = "Financeiro", id = idTituloReceita });

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", linkHref);

            if (!icon.isEmpty()) {

                htmlLink.InnerHtml = $"<i class=\"{icon}\"></i> ";

            }

            htmlLink.InnerHtml += textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());
        }

        /// <summary>
        /// Enviar cobrança de um titulo_receita por e-mail
        /// </summary>
        public static MvcHtmlString linkEnviarEmailCobrancaTitulo(this HtmlHelper helper, int idTituloReceita, string textoLink, string cssClass = "", string icon = "") {

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", "javascript:void(0);");

            htmlLink.Attributes.Add("onclick", $"TituloReceitaCobranca.abrirModalGeracaoEmailCobranca('{ idTituloReceita }')");

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Enviar e-mail de cobrança para o associado");

            if (!icon.isEmpty()) {

                htmlLink.InnerHtml = $"<i class=\"{icon}\"></i> ";

            }

            htmlLink.InnerHtml += textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());
        }
    }
}