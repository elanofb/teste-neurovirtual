using System.Text;

namespace System.Web.Mvc.Html {

	public static class LinkTablesDefault {

		//
		public static MvcHtmlString linkDeleteDefault(this HtmlHelper helper, object idDelete, string controllerName = "", string actionName = "excluir", string flagSistema = "N", string textoAposIcone = "") {
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			string url = urlHelper.Action(actionName, new { id = idDelete });

			if (!String.IsNullOrEmpty(controllerName)) {
				url = urlHelper.Action(actionName, controllerName, new { id = idDelete });
			}
            
			StringBuilder html = new StringBuilder();

		    if (flagSistema != "S") {
		        html.Append("<a href=\"javascript:void(0);\" title=\"Esta ação irá remover definitivamente esse registro. Apenas execute-a com absoluta certeza.\" data-toggle=\"tooltip\" data-url=\"" + url + "\" data-id=\"" + idDelete + "\" class=\"delete-default\">");
			    html.Append("<i class=\"fa fa-trash\"></i>");
			    html.Append(textoAposIcone);
			    html.Append("</a>");
            }

            return new MvcHtmlString(html.ToString());
		}

		//
		public static MvcHtmlString linkStatusDefault(this HtmlHelper helper, int idStatus, string ativo, string controllerName = "", string actionName = "alterar-status", string areaName = "") {
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			var url = urlHelper.Action(actionName, new { id = idStatus });
			
			if (!controllerName.isEmpty()) {
				url = urlHelper.Action(actionName, controllerName, new { id = idStatus });
			}
            
			if (!areaName.isEmpty()) {
				url = urlHelper.Action(actionName, controllerName, new { area=areaName, id = idStatus });
			}

			StringBuilder html = new StringBuilder();
            html.Append("<a href=\"javascript:void(0);\" data-toggle=\"tooltip\" title=\"Clique para alterar o status\" data-url=\"" + url + "\" data-id=\"" + idStatus + "\" class=\"ico-status bold " + (ativo == "S" ? "text-green" : "text-red") + "\">");
            html.Append((ativo == "S" ? "<i class='fa fa-check'></i> Ativo" : "<i class='fa fa-times'></i> Desativado"));
            html.Append("</a>");

            return new MvcHtmlString(html.ToString());
		}

        public static MvcHtmlString linkStatusDefault(this HtmlHelper helper, int idStatus, bool? ativo, string controllerName = "", string actionName = "alterar-status", string areaName = "") {
            
	        var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(actionName, new { id = idStatus });
            
	        if (!controllerName.isEmpty()) {
		        url = urlHelper.Action(actionName, controllerName, new { id = idStatus });
	        }
            
	        if (!areaName.isEmpty()) {
		        url = urlHelper.Action(actionName, controllerName, new { area=areaName, id = idStatus });
	        }

            StringBuilder html = new StringBuilder();
            html.Append("<a href=\"javascript:void(0);\" data-toggle=\"tooltip\" title=\"Clique para alterar o status\" data-url=\"" + url + "\" data-id=\"" + idStatus + "\" class=\"ico-status bold " + (ativo == true ? "text-green" : "text-red") + "\">");
            html.Append((ativo == true ? "<i class='fa fa-check'></i> Ativo" : "<i class='fa fa-times'></i> Desativado"));
            html.Append("</a>");

            return new MvcHtmlString(html.ToString());
        }

        public static MvcHtmlString linkStatusYesNo(this HtmlHelper helper, int idStatus, bool? ativo, string controllerName = "", string actionName = "alterar-status", int idReferencia = 0) {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(actionName, new { id = idStatus });
            if (!String.IsNullOrEmpty(controllerName)) {
                url = urlHelper.Action(actionName, controllerName, new { id = idStatus, idObjeto = idReferencia });
            }

            StringBuilder html = new StringBuilder();
            html.Append("<a href=\"javascript:void(0);\" data-toggle=\"tooltip\" title=\"Clique para alterar o status\" data-url=\"" + url + "\" data-id=\"" + idStatus + "\" class=\"ico-status bold " + (ativo == true ? "text-green" : "text-red") + "\">");
            html.Append((ativo == true ? "Sim" : "Não"));
            html.Append("</a>");

            return new MvcHtmlString(html.ToString());
        }

        public static MvcHtmlString linkStatusCustom(this HtmlHelper helper, bool? ativo) {
            
            var textoStatus = "Ativo";
            var iconeStatus = "fa-check";
            var corStatus = "green";

            if (ativo == false) {
                textoStatus = "Desativado";
                iconeStatus = "fa-times";
                corStatus = "red";
            }

            StringBuilder html = new StringBuilder();
            html.Append("<a href=\"javascript:void(0);\" class=\"text-"+ corStatus +"\">");
            html.Append("   <i class=\"fa "+ iconeStatus +"\"></i> ");
            html.Append(    textoStatus);
            html.Append("</a>");

            return new MvcHtmlString(html.ToString());
        }

        //
        public static MvcHtmlString linkDefaultEdit(this HtmlHelper helper, int idEdit, string flagSistema = "N") {
			
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			
			string actionPadrao = "editar";
			
			var url = urlHelper.Action(actionPadrao, new { id = idEdit });
			
			StringBuilder html = new StringBuilder();

            if (flagSistema == "S") {
                html.Append($"<a href=\"{ url }\">Protegido</a>");
                return new MvcHtmlString(html.ToString());
            }

            html.Append($"<a href=\"{ url }\" title=\"Editar Registro\" data-toggle=\"tooltip\"><i class=\"fa fa-edit\"></i></a>");

            return new MvcHtmlString(html.ToString());
		}
	}
}