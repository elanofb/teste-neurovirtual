using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace WEB.Helpers{

    public static class FlashMessage{

        //
        public static MvcHtmlString showFlashMessage(this HtmlHelper helper){
            StringBuilder html = new StringBuilder();

            html.AppendLine(getMessages(UtilMessage.TYPE_MESSAGE_SUCCESS));
            UtilMessage.listSuccess.Clear();

            //html.AppendLine(getMessages(UtilMessage.TYPE_MESSAGE_ERROR));
            html.AppendLine(getMessages(UtilMessage.TYPE_MESSAGE_DANGER));
            UtilMessage.listErrors.Clear();

            html.AppendLine(getMessages(UtilMessage.TYPE_MESSAGE_WARNING));
            UtilMessage.listWarnings.Clear();

            html.AppendLine(getMessages(UtilMessage.TYPE_MESSAGE_INFO));
            UtilMessage.listInfos.Clear();

            return new MvcHtmlString(html.ToString());
        }

        //Devolve uma string sem acentos
        public static MvcHtmlString showFlashMessage(this HtmlHelper helper, string typeMessage, string message){
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("alert");
            tagBuilder.AddCssClass(string.Concat("alert-", typeMessage));
            tagBuilder.AddCssClass("alert-dismissable");
			tagBuilder.InnerHtml += "<i class=\"fa "+FlashMessage.getIconMessage(typeMessage)+"\"></i>";
            tagBuilder.InnerHtml += ("<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button> <b>"+FlashMessage.getTitleMessage(typeMessage)+"</b>!" + message);
            return new MvcHtmlString(tagBuilder.ToString());
            
        }

        //
        private static string getMessages(string typeMessage) { 
            StringBuilder html = new StringBuilder();
            List<string> list = (List<string>)UtilMessage.TempData[typeMessage];
			if(list == null || list.Count == 0) return "";

			foreach (string message in list) {
                html.AppendLine(message).Append("<br />");
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("alert");
            tagBuilder.AddCssClass(string.Concat("alert-", typeMessage));
            tagBuilder.AddCssClass("alert-dismissable");
			tagBuilder.InnerHtml += "<i class=\"fa "+FlashMessage.getIconMessage(typeMessage)+"\"></i>";
            tagBuilder.InnerHtml += ("<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button> <b>"+FlashMessage.getTitleMessage(typeMessage)+"</b>!<br />");
			tagBuilder.InnerHtml += html.ToString();

            UtilMessage.TempData[typeMessage] = null;
			return new MvcHtmlString(tagBuilder.ToString()).ToHtmlString();
        }

        //
        private static string getTitleMessage(string typeMessage) {
            string className = "";
            if (typeMessage == UtilMessage.TYPE_MESSAGE_ERROR) {
                className = "Erro";
            }else if(typeMessage == UtilMessage.TYPE_MESSAGE_SUCCESS){
                className = "Sucesso";
            } else if (typeMessage == UtilMessage.TYPE_MESSAGE_INFO) {
                className = "Aviso";
            } else {
                className = "Atenção";
            }
            return className;
        }

        //
        private static string getIconMessage(string typeMessage) {
            string className = "";
            //if (typeMessage == UtilMessage.TYPE_MESSAGE_ERROR) {
            if (typeMessage == UtilMessage.TYPE_MESSAGE_DANGER) {
                className = "fa-ban";
            }else if(typeMessage == UtilMessage.TYPE_MESSAGE_SUCCESS){
                className = "fa-check";
            } else if (typeMessage == UtilMessage.TYPE_MESSAGE_INFO) {
                className = "fa-info";
            } else {
                className = "fa-info";
            }
            return className;
        }
    }
}
