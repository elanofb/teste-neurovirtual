using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace WEB.Helpers {

	public static class OrderHelper {

		//
		public static HtmlString linkOrder(this HtmlHelper helper, string texto, string campoOrder, AjaxOptions Options) {
			
			var ORequest = HttpContext.Current.Request;
			
			string[] keys = ORequest.QueryString.AllKeys.ToArray();

			string iconeOrder = "";

			RouteValueDictionary RotaParametros = new RouteValueDictionary();

			if (keys.Contains("orderCampo")) {
				string orderBusca = ORequest.QueryString["orderCampo"];
				if (orderBusca == campoOrder || orderBusca == String.Concat(campoOrder, "_desc")) {

					if (orderBusca.Contains("_desc")) {
						iconeOrder = "fa fa-angle-double-down";
					} else {
						campoOrder = String.Concat(campoOrder, "_desc");
						iconeOrder = "fa fa-angle-double-up";
					}
				}
			}

			RotaParametros.Add("orderCampo", campoOrder);

			foreach (string param in keys) {
				if (ORequest.QueryString[param] != null && param != "orderCampo") { 
					string valor = ORequest.QueryString[param];
					RotaParametros.Add(param, valor);
				}
			}
			
			var urlHelper = new UrlHelper(ORequest.RequestContext);
    
			string urlOrder = urlHelper.Action(null, null, RotaParametros);
			
			string linkOrder;

			if (Options == null) {
				
				linkOrder = String.Concat("<a href=\"", urlOrder, "\" class=\"text-white\"><i class=\"", iconeOrder,"\"></i> ", texto, "</a>");
				
				return new HtmlString(linkOrder);
			}

			linkOrder = String.Concat("<a data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-update=\"", Options.UpdateTargetId,"\"  href=\"", urlOrder, "\"><i class=\"", iconeOrder,"\"></i> ", texto, "</a>");

			return new HtmlString(linkOrder);
}

	}
}