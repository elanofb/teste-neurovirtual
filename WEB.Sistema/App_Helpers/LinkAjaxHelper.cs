using System;
using System.Text;
using System.Web.Mvc;

namespace WEB.Helpers {

	public static class LinkAjaxExtensions {

		//
		public static MvcHtmlString linkAjaxStatus(this HtmlHelper helper, int id, string ativo, string urlAjax = "", string extraClasses = "") {
			StringBuilder htm = new StringBuilder();
			
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			string actionPadrao = "alterar-status";
			
			urlAjax = String.IsNullOrEmpty(urlAjax) ? urlHelper.Action(actionPadrao) : urlAjax;

			htm.Append("<a href='javascript:void(0);' class='" + String.Concat("ico-status ", (ativo.Equals("S") ? "on " : "off "), extraClasses) + "' data-id='" + id + "' data-url='" + urlAjax + "' data-toggle='tooltip' title='Alterar Status'>");
			htm.Append((ativo.Equals("S") ? "Sim" : "Não"));
			htm.Append("</a>");

			return new MvcHtmlString(htm.ToString());
		}

		public static MvcHtmlString linkAjaxStatus(this HtmlHelper helper, int id, bool? ativo, string urlAjax = "", string extraClasses = "", string fnCallBack = "") {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			
			string actionPadrao = "alterar-status";

			urlAjax = String.IsNullOrEmpty(urlAjax) ? urlHelper.Action(actionPadrao) : urlAjax;

			htm.Append("<a href='javascript:void(0);' class='" + String.Concat("ico-status ", (ativo == true ? "on " : "off "), extraClasses) + "' data-id='" + id + "' data-url='" + urlAjax + "' data-toggle='tooltip' data-fncallback='" + fnCallBack + "' title='Alterar Status'>");
			htm.Append((ativo == true ? "Sim" : "Não"));
			htm.Append("</a>");

			return new MvcHtmlString(htm.ToString());
		}

		//
		public static MvcHtmlString linkAjaxExcluir(this HtmlHelper helper, int id, string flagSistema = "N", string urlAjax = "", string extraClasses = "", string fnCallBack = "", string descricao = "") {

			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			
			string actionPadrao = "excluir";

			urlAjax = String.IsNullOrEmpty(urlAjax) ? urlHelper.Action(actionPadrao) : urlAjax;

			if (flagSistema != ("S")) {
				htm.Append("<a href='javascript:void(0);' class='" + String.Concat("delete-default ", extraClasses) + "' data-id='" + id + "' data-url='" + urlAjax + "' data-toggle='tooltip' data-fncallback='" + fnCallBack + "' title='Remover Registro'>");
				htm.Append("<i class=\"far fa-trash-alt\"></i>");
				htm.Append(descricao);
				htm.Append("</a>");
			}

			return new MvcHtmlString(htm.ToString());
		}

	}
}