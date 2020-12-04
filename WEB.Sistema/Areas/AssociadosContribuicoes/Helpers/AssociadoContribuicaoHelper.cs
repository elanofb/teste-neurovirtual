using System;
using System.Text;
using System.Web.Mvc;
using DAL.AssociadosContribuicoes;
using WEB.Areas.AssociadosContribuicoes.ViewModels;
using WEB.Areas.Financeiro.Helpers;

namespace WEB.Areas.AssociadosContribuicoes.Helpers {

    public static class AssociadoContribuicaoHelper {

        //
        public static MvcHtmlString linkRecibo(this HtmlHelper helper, int idAssociadoContribuicao, string textoLink, string cssClass = "") {

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("exibir-recibo", "ReciboContribuicao", new { area = "Recibos", i = UtilCrypt.toBase64Encode(idAssociadoContribuicao) });

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", linkHref);

            htmlLink.Attributes.Add("target", "_blank");

            htmlLink.Attributes.Add("title", "Visualizar recibo de pagamento.");

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {
                htmlLink.AddCssClass(cssClass);
            }

            return new MvcHtmlString(htmlLink.ToString());
        }

        //
        public static MvcHtmlString linkEnviarEmailCobranca(this HtmlHelper helper, AssociadoContribuicaoResumoVW AssociadoContribuicao, string textoLink, string cssClass = "") {
            
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", "javascript:void(0);");

            htmlLink.Attributes.Add("onclick", $"AssociadoContribuicaoCobranca.abrirModalGeracaoEmailCobranca('{ AssociadoContribuicao.idContribuicao }', '{ AssociadoContribuicao.id }')");
            
            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Enviar e-mail de cobrança para o associado");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());
        }

        //
        public static MvcHtmlString linkEnviarEmailCobranca(this HtmlHelper helper, AssociadoContribuicao AssociadoContribuicao, string textoLink, string cssClass = "") {
            
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", "javascript:void(0);");

            htmlLink.Attributes.Add("onclick", $"AssociadoContribuicaoCobranca.abrirModalGeracaoEmailCobranca('{ AssociadoContribuicao.idContribuicao }', '{ AssociadoContribuicao.id }')");
            
            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Enviar e-mail de cobrança para o associado");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());
        }

        //
        public static MvcHtmlString linkExcluirContribuicao(this HtmlHelper helper, int idAssociadoContribuicao, string textoLink, string cssClass = "") {

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("partial-excluir-contribuicao", "associadocontribuicaoexclusao", new { area = "associadoscontribuicoes", id = idAssociadoContribuicao });

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", "javascript:void(0);");

            htmlLink.Attributes.Add("onclick", "DefaultSistema.showModal('" + linkHref + "')");

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Remover essa cobrança do associado");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());
        }

        //
        public static MvcHtmlString linkConcederIsencao(this HtmlHelper helper, int idAssociadoContribuicao, string textoLink, string cssClass = "") {

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var linkHref = urlHelper.Action("partial-isencao-contribuicao", "associadocontribuicaoisencao", new { area = "associadoscontribuicoes", id = idAssociadoContribuicao });

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", "javascript:void(0);");

            htmlLink.Attributes.Add("onclick", "DefaultSistema.showModal('" + linkHref + "')");

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("title", "Conceder Isenção dessa cobrança para o associado");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());
        }

        /// <summary>
        /// Link para um boleto bancario
        /// </summary>
        public static MvcHtmlString linkBoleto(this HtmlHelper helper, string urlBoleto, string textoLink, string cssClass = "") {

            TagBuilder htmlLink = new TagBuilder("a");

            htmlLink.Attributes.Add("href", urlBoleto);

            htmlLink.Attributes.Add("data-toggle", "tooltip");

            htmlLink.Attributes.Add("target", "_blank");

            htmlLink.Attributes.Add("title", "Visualizar o boleto bancário");

            htmlLink.InnerHtml = textoLink;

            if (!string.IsNullOrEmpty(cssClass)) {

                htmlLink.AddCssClass(cssClass);

            }

            return new MvcHtmlString(htmlLink.ToString());
        }
        
        /// <summary>
        /// Gerar o menu de acoes para uma contribuica
        /// </summary>
        public static MvcHtmlString menuAcoes(this HtmlHelper helper, AssociadoContribuicaoItemLista Cobranca) {

            StringBuilder html = new StringBuilder();

            html.AppendLine("<button class=\"btn btn-default btn-white-sm dropdown-toggle\" data-toggle=\"dropdown\"> Ações <span class=\"caret\"></span></button>");

            html.AppendLine("<ul class=\"dropdown-menu dropdown-menu-right\">");


            if (!Cobranca.AssociadoContribuicao.flagQuitado() && !Cobranca.AssociadoContribuicao.flagTemParcelamento()){

                html.AppendLine($"<li>{ helper.linkRegistrarPagamentoTitulo(Cobranca.AssociadoContribuicao.idTituloReceita.toInt(), "Registrar pagamento") }</li>");


                html.AppendLine($"<li>{ helper.linkParcelarTitulo(Cobranca.AssociadoContribuicao.idTituloReceita.toInt(), "Parcelar cobrança") }</li>");

            }
                
            html.AppendLine($"<li>{helper.linkDetalheTitulo(Cobranca.AssociadoContribuicao.idTituloReceita.toInt(), "Detalhes Cobrança")}</li>");

            if (Cobranca.AssociadoContribuicao.flagPodeIsentar()){

                html.AppendLine($"<li>{ helper.linkConcederIsencao(Cobranca.AssociadoContribuicao.id, "Conceder isenção") }</li>");
            }

            if (Cobranca.AssociadoContribuicao.dtPagamento.HasValue){

                html.AppendLine($"<li>{ helper.linkRecibo(Cobranca.AssociadoContribuicao.id, "Recibo pagamento") }</li>");

            }

            html.AppendLine("<li ole=\"separator\" class=\"divider\"></li>");

            html.AppendLine($"<li>{ helper.linkExcluirContribuicao(Cobranca.AssociadoContribuicao.id, "Excluir cobrança") }</li>");

            html.AppendLine("</ul>");
    

        
            return new MvcHtmlString(html.ToString());
        }
    }
}