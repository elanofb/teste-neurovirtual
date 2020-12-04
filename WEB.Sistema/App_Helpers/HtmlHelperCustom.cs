using System;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;
using PagedList;

namespace WEB.Helpers {

	public static class HtmlHelperCustom {

        public static MvcHtmlString buttonSearch(this HtmlHelper helper, string valorPadrao = "", string cssClass = "", string cssClassButton = "") {

            dynamic divInputGroup = new TagBuilder("div");

            divInputGroup.AddCssClass("input-group input-group-sm");

			dynamic inputText = new TagBuilder("input");

            inputText.Attributes.Add("type", "text");

            inputText.Attributes.Add("name", "valorBusca");
	        
	        inputText.Attributes.Add("id", "valorBusca");

            inputText.Attributes.Add("title", "Utilize os filtros para localizar os registros que desejar.");

            inputText.Attributes.Add("data-toggle", "tooltip");

            inputText.Attributes.Add("value", valorPadrao);

            inputText.AddCssClass("form-control input-sm");

            if ((!string.IsNullOrEmpty(cssClass))) {
				inputText.AddCssClass(cssClass);
			}

			dynamic divGroupBtn = new TagBuilder("div");
			divGroupBtn.AddCssClass("input-group-btn");

			dynamic buttonSubmit = new TagBuilder("button");
			buttonSubmit.Attributes.Add("name", "search");
            buttonSubmit.Attributes.Add("value", "search");
			buttonSubmit.AddCssClass("btn btn-sm btn-primary");
            buttonSubmit.AddCssClass(cssClassButton);
			buttonSubmit.InnerHtml = "<i class=\"fa fa-search\"></i>";

			divGroupBtn.InnerHtml = buttonSubmit.ToString();
			divInputGroup.InnerHtml = string.Concat(inputText.ToString(), divGroupBtn.ToString());

			return new MvcHtmlString(divInputGroup.ToString());

		}

        public static MvcHtmlString buttonFlagTipoSaida(this HtmlHelper helper, string valorPadrao = "", string cssClass = "", string cssClassButton = "", bool showBlank = false) {

	        dynamic divInputGroup = new TagBuilder("div");

            divInputGroup.AddCssClass("input-group input-group-sm");

			dynamic selectText = new TagBuilder("select");

	        selectText.Attributes.Add("type", "text");

	        selectText.Attributes.Add("name", "flagTipoSaida");
	        
	        selectText.Attributes.Add("id", "flagTipoSaida");

	        selectText.Attributes.Add("title", "Utilize os filtros para localizar os registros que desejar e selecione o tipo de saída.");

	        selectText.Attributes.Add("data-toggle", "tooltip");

	        if (showBlank){
		        TagBuilder option = new TagBuilder("option");
		        option.MergeAttribute("value", "");
		        option.InnerHtml = "...";
		        
		        selectText.InnerHtml += option.ToString();
	        }

	        foreach (var selectListItem in TipoSaidaHelper.selectListTipoSaida(valorPadrao)){
		        TagBuilder option = new TagBuilder("option");
		        option.MergeAttribute("value", selectListItem.Value);
		        option.InnerHtml = selectListItem.Text;



		        if (selectListItem.Selected) {
			        option.MergeAttribute("selected", "selected");
		        }
		        selectText.InnerHtml += option.ToString();
	        }

	        selectText.AddCssClass("form-control input-sm");

            if ((!string.IsNullOrEmpty(cssClass))) {
	            selectText.AddCssClass(cssClass);
			}

			dynamic divGroupBtn = new TagBuilder("div");
			divGroupBtn.AddCssClass("input-group-btn");

			dynamic buttonSubmit = new TagBuilder("button");
			buttonSubmit.Attributes.Add("name", "search");
            buttonSubmit.Attributes.Add("value", "search");
			buttonSubmit.AddCssClass("btn btn-sm btn-primary");
            buttonSubmit.AddCssClass(cssClassButton);
			buttonSubmit.InnerHtml = "<i class=\"fa fa-search\"></i>";

			divGroupBtn.InnerHtml = buttonSubmit.ToString();
			divInputGroup.InnerHtml = string.Concat(selectText.ToString(), divGroupBtn.ToString());

			return new MvcHtmlString(divInputGroup.ToString());

		}

		//
		public static MvcHtmlString paginarRegistros<T>(this HtmlHelper helper, IPagedList<T> Model, MvcHtmlString pager) {
			StringBuilder htm = new StringBuilder();
			htm.Append("<div style=\"margin-top:7px;\">");
			htm.Append("	<div class=\"col-sm-5 col-xs-6 no-padding-left\">");
			htm.Append("		<div class=\"dataTables_info\">Exibindo ").Append(Model.TotalItemCount < Model.PageSize ? Model.TotalItemCount : Model.PageSize).Append(" de ").Append(Model.TotalItemCount).Append(" registros.").Append("</div>");
			htm.Append("	</div>");
			htm.Append("	<div class=\"col-sm-2 hidden-xs\">");
			htm.Append("		<div class=\"dataTables_info\">Página ").Append(Model.PageNumber).Append(" de ").Append(Model.PageCount).Append(".</div>");
			htm.Append("	</div>");
			htm.Append("	<div class=\"col-sm-5 col-xs-6 no-padding-right\">");
			htm.Append("		<div class=\"dataTables_paginate paging_bootstrap\">");
			htm.Append(				"<div class=\"pull-right\">");
			htm.Append(pager);
			htm.Append(				"</div>");
			htm.Append("		</div>");
			htm.Append("	</div>");
			htm.Append("</div>");
            htm.Append("<div class=\"clearfix\"></div><br />");


			return new MvcHtmlString(htm.ToString());
		}

		//
		public static MvcHtmlString paginarRegistrosSemPaginas<T>(this HtmlHelper helper, IPagedList<T> Model, MvcHtmlString pager) {
			StringBuilder htm = new StringBuilder();
			htm.Append("<div>");
			htm.Append("	<div class=\"col-xs-4 no-padding\">");
			htm.Append("		<div class=\"dataTables_info\">Exibindo ").Append(Model.TotalItemCount < Model.PageSize ? Model.TotalItemCount : Model.PageSize).Append(" de ").Append(Model.TotalItemCount).Append(" registros.").Append("</div>");
			htm.Append("	</div>");
			htm.Append("	<div class=\"col-xs-8 no-padding\">");
			htm.Append("		<div class=\"dataTables_paginate paging_bootstrap\">");
			htm.Append(				pager);
			htm.Append("		</div>");
			htm.Append("	</div>");
			htm.Append("</div>");

			return new MvcHtmlString(htm.ToString());
		}

		//
		public static string exibirValor(this decimal? valorPadrao) {
			string html;
			
			if (valorPadrao.HasValue) {
				html = valorPadrao.Value.ToString("C");
			} else {
				html = new decimal(0).ToString("C");	
			}

			return html;
		}


		//
		public static string exibirData(this DateTime dtPadrao, bool incluirHorario = false) {
			string html = "-";
			if (dtPadrao > DateTime.MinValue) {
				
				html = dtPadrao.ToShortDateString();

				if (incluirHorario) {
					html = String.Concat(html, " ", dtPadrao.ToShortTimeString());
				}
			}

			return new MvcHtmlString(html).ToString();
		}

		//
		public static MvcHtmlString widgetIon(this HtmlHelper helper, string title, string subTitle, string bgBox, string ionIcon, string linkDetalhe = "") {
			StringBuilder htm = new StringBuilder();
			htm.Append("<div class=\"\">");
			htm.Append(		"<div class=\"small-box "+bgBox+"\">");
			htm.Append(			"<div class=\"inner\">");
			htm.Append(				"<h3>"+title+"</h3>");
			htm.Append(				"<p>"+subTitle+"</p>");
			htm.Append(			"</div>");
			htm.Append(			"<div class=\"icon\"><i class=\"ion "+ionIcon+"\"></i></div>");
			
			if (!String.IsNullOrEmpty(linkDetalhe)) { 
				htm.Append(			"<a href=\""+linkDetalhe+"\" class=\"small-box-footer\">");
				htm.Append(				"Detalhes <i class=\"fa fa-arrow-circle-right\"></i>");
				htm.Append(			"</a>");
			}
			htm.Append(		"</div>");
			htm.Append("</div>");

			return new MvcHtmlString(htm.ToString());
		}

		//
		public static MvcHtmlString boxInfo(this HtmlHelper helper, string title, string info, string classBox, string classInfo="") {

			StringBuilder htm = new StringBuilder();
			
			htm.Append("<div class=\""+classBox+"\">");
				htm.Append("<label class=\"data-title\">").Append(title).Append("</label>");
				htm.Append("<label class=\"data-info "+classInfo+"\">").Append( String.IsNullOrEmpty(info)? "-": info ).Append("</label>");
			htm.Append("</div>");

			return new MvcHtmlString(htm.ToString());
		}

        //
		public static MvcHtmlString boxInfoEditable(this HtmlHelper helper, string type, string url, int id, string name, string value, string title, string info, string classBox, bool somenteLeitura = false, string classInfo="", string classEdiTable = "info-editavel", string dataAlt = "") {

		    if (somenteLeitura)
		    {
		        return HtmlHelperCustom.boxInfo(helper, title, info, classBox, classInfo);
		    }

		    var htm = new StringBuilder();
			
			htm.Append("<div class=\""+classBox+"\">");
				htm.Append("<label class=\"data-title\">").Append(title).Append("</label>");
		            htm.Append("<label class=\"data-info " + classInfo + "\">")
		                .Append("<a href=\"#\" data-original-title=\"\" title=\"\" class=\"" + classEdiTable +" editable editable-click\"");
		    
                            if (!String.IsNullOrEmpty(dataAlt))
		                    {
		                        htm.Append(" data-alt=\""+ dataAlt +"\"");
		                    }	
	    
                            htm.Append(" data-type=\""+ type +"\"")
                            .Append(" data-source=\""+ url +"\"")
                            .Append(" data-pk=\""+ id +"\"")
                            .Append(" data-name=\""+ name +"\"")
                            .Append(" data-value=\""+ value +"\"")
                            .Append(" data-title=\""+ title +"\">")
                            .Append( String.IsNullOrEmpty(info)? "-": info )
                        .Append("</a>")
                    .Append("</label>");
			htm.Append("</div>");


			return new MvcHtmlString(htm.ToString());
		}

        public static MvcHtmlString boxInfo(this HtmlHelper helper, string info, string cssClasses = "") {

            StringBuilder htm = new StringBuilder();

            info = String.IsNullOrEmpty(info) ? "-" : info;

            htm.Append("<input type=\"text\" class=\"form-control input-sm  " + cssClasses + "\" readonly=\"readonly\" value=\"" + info + "\">");

            return new MvcHtmlString(htm.ToString());
        }

        public static MvcHtmlString labelTrueFalse(this HtmlHelper helper, bool? ativo, string text, string cssClasses = "") {

            StringBuilder htm = new StringBuilder();

            htm.Append("<span class=\"text-")
                .Append(ativo == true ? "green " : "red ")
                .Append(cssClasses)
                .Append(" no-pointer\">")
                .Append("<i class=\"fa fa-")
                .Append(ativo == true ? "check" : "times")
                .Append("\"></i> ")
                .Append(text)
                .Append("</span>");

            return new MvcHtmlString(htm.ToString());
        }

        //
        public static MvcHtmlString labelStatus(this HtmlHelper helper, string ativo) {

			StringBuilder htm = new StringBuilder();
			
			htm.Append("<button class=\"btn btn-mini btn-")
                .Append(ativo == "S" ? "success" : "danger")
                .Append(" no-pointer\">")
                .Append(ativo == "S" ? "Sim" : "Não")
                .Append("</button>");

			return new MvcHtmlString(htm.ToString());
		}

		/// <summary>
        /// Exibir a descrição de status de um registro de forma visual: verde para ativos e vermelho para inativos
        /// </summary>
		public static MvcHtmlString badgeStatus(this HtmlHelper helper, string ativo) {

			StringBuilder htm = new StringBuilder();
			
			htm.Append("<span class=\"badge bg-")
                .Append(ativo == "S" ? "green" : "red")
                .Append(" no-pointer\">")
                .Append(ativo == "S" ? "Ativo" : "Desativado")
                .Append("</button>");

			return new MvcHtmlString(htm.ToString());
		}

		/// <summary>
        /// Exibir a descrição de status de um registro de forma visual: verde para ativos e vermelho para inativos
        /// </summary>
		public static MvcHtmlString badgeStatus(this HtmlHelper helper, bool? ativo) {

			StringBuilder htm = new StringBuilder();
			
			htm.Append("<span class=\"badge bg-")
                .Append(ativo == true ? "green" : "red")
                .Append(" no-pointer\">")
                .Append(ativo == true ? "Ativo" : "Desativado")
                .Append("</button>");

			return new MvcHtmlString(htm.ToString());
		}


        //
        public static MvcHtmlString headerBox(this HtmlHelper helper, string titulo, string icon, bool flagMinimizer = false, string linkNovo = "", string textoLinkNovo = "Novo Registro") {

            StringBuilder htm = new StringBuilder();

            htm.Append("<div class=\"box-header with-border\">");
            htm.Append("	<h3 class=\"box-title fs-14\"><i class=\"" + icon + "\"></i> ").Append(titulo).Append("</h3>");
            htm.Append("	<div class=\"box-tools pull-right\">");

            if (!linkNovo.IsEmpty()) {
                htm.Append("		<a type=\"button\" href=\""+linkNovo+"\" class=\"btn btn-box-tool\"><i class=\"fa fa-plus\"></i> "+textoLinkNovo+"</a>");
            }

            if (flagMinimizer) {
				htm.Append("		<button type=\"button\" class=\"btn btn-box-tool\" data-widget=\"collapse\"><i class=\"far fa-minus\"></i></button>");
			}

            htm.Append("	</div>");
            htm.Append("</div>");

            return new MvcHtmlString(htm.ToString());
        }

        //
		public static MvcHtmlString headerBoxResultados(this HtmlHelper helper, string titulo, string icon, string textAdicionar = "Adicionar", string urlAdicionar = "editar") {
			StringBuilder htm = new StringBuilder();

			htm.Append("<div class=\"box-header with-border\">");
			htm.Append("	<h3 class=\"box-title\"><i class=\""+icon+"\"></i> ").Append(titulo).Append("</h3>");
			htm.Append("	<div class=\"box-tools pull-right\">");
			htm.Append("    <a href=\"").Append(urlAdicionar).Append("\" class=\"btn btn-box-tool\"><i class=\"far fa-plus-circle\"></i> ").Append(textAdicionar).Append("</a>");
            htm.Append("		<button type=\"button\" class=\"btn btn-box-tool\" data-widget=\"collapse\"><i class=\"far fa-minus\"></i></button>");
			htm.Append("	</div>");
			htm.Append("</div>");

			return new MvcHtmlString(htm.ToString());
		}

        /// <summary>
        /// Classe css textred ou text-green
        /// </summary>
	    public static string textGreenOrRed(this HtmlHelper helper, bool? flagOpcao){

            return flagOpcao == true ? "text-green" : "text-red";
        }

        /// <summary>
        /// Classe css textred ou text-green
        /// </summary>
	    public static string textGreenOrRed(this HtmlHelper helper, string flagOpcao){

            return flagOpcao == "S" ? "text-green" : "text-red";
        }

        /// <summary>
        /// Classe css textred ou text-green
        /// </summary>
	    public static string iconCheckOrBan(this HtmlHelper helper, bool? flagOpcao){

            return flagOpcao == true ? "fa-check" : "fa-ban";
        }
        /// <summary>
        /// Classe css fa-check ou fa-ban
        /// </summary>
	    public static string iconCheckOrBan(this HtmlHelper helper, string flagOpcao){

            return flagOpcao == "S" ? "fa-check" : "fa-ban";
        }

        //
        public static MvcHtmlString linkEditable(this HtmlHelper helper, object pk, string name, object value, string title, string urlEdicao, string type = "text", string dataSource = "", string textLink = "", int maxlength = 0, int width = 0, string cssClass = "", string alt = "", string placement = "", string callbackSuccess = "", bool flagCache = false) {

            dynamic tagBuild = new TagBuilder("a");

            tagBuild.Attributes.Add("href", "javascript:;");

            tagBuild.Attributes.Add("data-pk", pk.ToString());

            tagBuild.Attributes.Add("data-name", name);

            tagBuild.Attributes.Add("data-value", value.stringOrEmpty());

            tagBuild.Attributes.Add("data-title", title);

            tagBuild.Attributes.Add("data-cache", flagCache? "true": "false");

            tagBuild.AddCssClass("info-editavel-default editable-click");

            if (!alt.IsEmpty()){
                tagBuild.Attributes.Add("data-alt", alt);
            }

            if (!type.IsEmpty()){
                tagBuild.Attributes.Add("data-type", type);
            }

            if (!placement.IsEmpty()){
                tagBuild.Attributes.Add("data-placement", placement);
            }

            if (!callbackSuccess.IsEmpty()){
                tagBuild.Attributes.Add("data-callback-success", callbackSuccess);
            }

            if (width > 0){
                tagBuild.Attributes.Add("data-width", width.stringOrEmpty());
            }

            if (maxlength > 0){
                tagBuild.Attributes.Add("data-maxlength", maxlength.ToString());
            }

            if (!urlEdicao.IsEmpty()){
                tagBuild.Attributes.Add("data-url", urlEdicao);
            }

            if (!dataSource.IsEmpty()){
                tagBuild.Attributes.Add("data-source", dataSource);
            }

            if (!string.IsNullOrEmpty(cssClass)) {

                tagBuild.AddCssClass(cssClass);

            }

            textLink = textLink.isEmpty() ? value.stringOrEmpty() : textLink;

            tagBuild.InnerHtml = textLink;

            return new MvcHtmlString(tagBuild.ToString());
        }

	}
}

