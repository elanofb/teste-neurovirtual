using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DAL.AssociadosContribuicoes;
using DAL.AssociadosContribuicoes.DTO;
using WEB.Areas.AssociadosContribuicoes.ViewModels;

namespace WEB.Areas.AssociadosContribuicoes.Extensions {

    public static class AssociadoContribuicaoExtensions {

        //Descricao do tipo de contribuicao da associacao utilizadora do sistema
        public static string cssBgSituacao(this AssociadoContribuicao OAssociadoContribuicao) {

            if (OAssociadoContribuicao == null) {
                return "";
            }

            if (OAssociadoContribuicao.flagIsento == true) {
                return "bg-yellow";
            }

            if (OAssociadoContribuicao.dtPagamento.HasValue) {
                return "bg-green";
            }

            if (OAssociadoContribuicao.flagEmAtraso()) {
                return "bg-red";
            }


            return "bg-gray";
        }

        //Descricao do tipo de contribuicao da associacao utilizadora do sistema
        public static string cssBgSituacao(this AssociadoContribuicaoDadosBasicos OAssociadoContribuicao) {

            if (OAssociadoContribuicao == null) {
                return "";
            }

            if (OAssociadoContribuicao.flagIsento == true) {
                return "bg-yellow";
            }

            if (OAssociadoContribuicao.dtPagamento.HasValue) {
                return "bg-green";
            }

            if (OAssociadoContribuicao.flagEmAtraso()) {
                return "bg-red";
            }


            return "bg-white";
        }

        //Descricao do tipo de contribuicao da associacao utilizadora do sistema
        public static string cssBorderSituacao(this AssociadoContribuicaoResumoVW OItem) {

            if (OItem == null || OItem.id == 0) {
                return "";
            }

            if (OItem.flagIsento == true) {
                return "border-yellow";
            }

            if (!OItem.flagQuitado() && OItem.flagVencido()) {
                return "border-red";
            }

            if (OItem.flagQuitado()) {
                return "border-green";
            }

            return "";
        }

        /// <summary>
        /// Exibir detalhe HTML para item de cobrança de associado
        /// </summary>
        public static IHtmlString exibirDetalhes(this HtmlHelper helper, AssociadoContribuicaoItemLista OItem) {

            var htmlBuilder = new StringBuilder();

            if (OItem.AssociadoContribuicao.id == 0) {
                return new MvcHtmlString("<strong>Cobrança não realizada</strong>");
            }

            if (OItem.AssociadoContribuicao.flagIsento == true) {
                return new MvcHtmlString($"<strong>Isento</strong> <span class=\"fs-10 text-italic\">({(OItem.AssociadoContribuicao.motivoIsencao.isEmpty()? "Motivo não registrado": OItem.AssociadoContribuicao.motivoIsencao)})</span>");

            }

            if (OItem.AssociadoContribuicao.dtPagamento.HasValue) {

                htmlBuilder.Append($"<strong>{OItem.valorAtualFinal.ToString("C")}</strong>");

                htmlBuilder.Append($"<small class=\"text-italic fs-10\">Pago ({OItem.AssociadoContribuicao.dtPagamento.exibirData()})</small>");

                return new MvcHtmlString(htmlBuilder.ToString());
            }

            htmlBuilder.Append($"<strong>{OItem.valorAtualFinal.ToString("C")} </strong>");

            decimal valorPago = OItem.AssociadoContribuicao.valorTotalRecebido.GetValueOrDefault();

            htmlBuilder.Append($"<small class=\"text-italic fs-10\">{(valorPago > 0? $" - {valorPago.ToString("C")} pago": "Em aberto")}</small>");

            if (OItem.AssociadoContribuicao.flagTemParcelamento()){

                decimal valorParcela = decimal.Divide(OItem.valorAtualFinal, new decimal(OItem.AssociadoContribuicao.qtdeParcelas.toInt()));

                htmlBuilder.Append($"<br /><strong>Parcelado {OItem.AssociadoContribuicao.qtdeParcelas}X {valorParcela.ToString("C")}</strong>");
            }


            if (OItem.AssociadoContribuicao.flagDescontoAntecipacao == true) {

                UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

                htmlBuilder.Append($"<br/><a href=\"javascript:void(0)\" onclick=\"DescontoAntecipacao.exibirModalDesconto(this)\" data-url=\"{urlHelper.Action("modal-detalhe-descontos", "DescontoAntecipacao", new {area = "Financeiro", OItem.AssociadoContribuicao.idTituloReceita})}\" class=\"text-italic fs-10\">Desconto por antecipação</a>");

            }

            return new MvcHtmlString(htmlBuilder.ToString());
        }

    }
}