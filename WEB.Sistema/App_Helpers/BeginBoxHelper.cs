using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WEB.Helpers {

    public class BeginBoxHelper : IDisposable{
        protected HtmlHelper _helper;

        private string box_class = "", body_style = "", fa_icon = "", flag_row = "", flag_btn_minimizer = "S", box_style = "default", html_box_tool = "";

        public BeginBoxHelper(HtmlHelper helper, string title, object attributes = null) {

            var atributos = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(attributes));
            if (atributos != null) {

                box_class = atributos.SelectToken("box_class").isEmpty() ? box_class : "class=\"" + atributos.SelectToken("box_class") + "\"";

                fa_icon = atributos.SelectToken("faIcon").isEmpty() ? fa_icon : "<i class=\"fa fa-" + atributos.SelectToken("faIcon") + "\"></i> ";

                body_style = atributos.SelectToken("body_style").isEmpty() ? body_style : "style=\"" + atributos.SelectToken("body_style") + "\"";

                flag_row = atributos.SelectToken("flag_row")?.ToString() ?? flag_row;

                html_box_tool = atributos.SelectToken("html_box_tool")?.ToString() ?? html_box_tool;
                
                flag_btn_minimizer = atributos.SelectToken("flag_btn_minimizer")?.ToString() ?? flag_btn_minimizer;

                box_style = atributos.SelectToken("box_style")?.ToString() ?? box_style;
            }

            _helper = helper;
            _helper.ViewContext.Writer.Write(
                "<div "+ box_class + ">" 
                    + (flag_row == "S" ? "<div class=\"row\">" : "")
                        + "<div class=\"box box-"+ box_style + "\">" 
                            + "<div class=\"box-header with-border\">"
                                + "<h3 class=\"box-title\">"+ fa_icon + "<span>"+ title + "</span></h3>"
                                + "<div class=\"box-tools pull-right\">"
                                    + html_box_tool
                                    + (flag_btn_minimizer == "S" ? "<button type=\"button\" class=\"btn btn-box-tool\" data-widget=\"collapse\"><i class=\"far fa-minus\"></i></button>" : "")

                                + "</div>"
                            + "</div> "
                            + "<div class=\"box-body\" "+ body_style + ">");
        }

        public void Dispose(){
            _helper.ViewContext.Writer.Write("" 
                            + "</div>" 
                        + "</div>"
                    + (flag_row == "S" ? "</div>" : "")
                + "</div>");
        }
    }
}

