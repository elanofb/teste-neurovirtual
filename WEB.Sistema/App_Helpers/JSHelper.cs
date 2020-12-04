using System;
using System.Text;
using System.Web.Mvc;

namespace WEB.Helpers {

	public static class JSHelper {

		//
		public static MvcHtmlString includeDatePicker(this HtmlHelper helper) {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/bootstrap-datepicker/css/datepicker3.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js?v=1")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/bootstrap-datepicker/js/locales/bootstrap-datepicker.pt-BR.js")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}
		
		//
		public static MvcHtmlString includeDateTimePicker(this HtmlHelper helper) {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.pt-BR.js")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}

		//
		public static MvcHtmlString includeDateRangePicker(this HtmlHelper helper) {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("\t<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses("js/associatec/plugins/bootstrap-daterangepicker/daterangepicker.css"))).Append("\" rel=\"stylesheet\" />\n");
			htm.Append("\t\t<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses("js/associatec/plugins/bootstrap-daterangepicker/moment.js"))).Append("\"></script>\n");
			htm.Append("\t\t<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses("js/associatec/plugins/bootstrap-daterangepicker/daterangepicker.js"))).Append("\"></script>\n");

			return new MvcHtmlString(htm.ToString());
		}
		
		//
		public static MvcHtmlString includeBootstrapMultiselect(this HtmlHelper helper) {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/bootstrap-multiselect/bootstrap-multiselect.css")).Append("\" rel=\"stylesheet\" />");
            htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "css/associatec/multiselect-custom.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/bootstrap-multiselect/bootstrap-multiselect.js")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}

		//
		public static MvcHtmlString includeDataTable(this HtmlHelper helper) {

			StringBuilder htm = new StringBuilder();

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/datatables/jquery.dataTables.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/datatables/extensions/ColVis/css/dataTables.colVis.min.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/datatables/jquery.dataTables.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/datatables/extensions/ColVis/js/dataTables.colVis.min.js")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}

		//
		public static MvcHtmlString includeSelect2(this HtmlHelper helper) {

            StringBuilder htm = new StringBuilder();

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/autocomplete/select2-4.0/css/select2.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/autocomplete/select2-4.0/js/select2.full.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/autocomplete/autocomplete.js")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}


		//Inclusao da biblioteca padrão bootstrap
		public static MvcHtmlString includeBootstrapEditable(this HtmlHelper helper) {

            StringBuilder htm = new StringBuilder();

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/bootstrap-editable/bootstrap3-editable/css/bootstrap-editable.css")).Append("\" rel=\"stylesheet\" />");

            htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/bootstrap-editable/bootstrap3-editable/js/bootstrap-editable.min.js")).Append("\"></script>");

            htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/editable-custom.js?v=1.0")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}

		//Inclusao da biblioteca padrão jquery.loading
		public static MvcHtmlString includeJqueryLoading(this HtmlHelper helper) {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/jquery.loading/jquery.loading.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/jquery.loading/jquery.loading.js")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}

		
		//Inclusao da biblioteca padrão dhtmlxDiagram
		public static MvcHtmlString includeDhtmlxDiagram(this HtmlHelper helper) {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/dhtmlx-diagram/diagram.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/dhtmlx-diagram/diagram.js")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}


		//Inclusao da biblioteca
		public static MvcHtmlString includeToastCSS(this HtmlHelper helper) {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/toastr/toastr.css")).Append("\" rel=\"stylesheet\" />");

			return new MvcHtmlString(htm.ToString());
		}

		//Inclusao da biblioteca
		public static MvcHtmlString includeToastJS(this HtmlHelper helper) {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/toastr/toastr.min.js")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}
	
		/// <summary>
        /// Inclusao de arquivos CSS necessarios para utilizacao da editor FROALA
        /// </summary>
        /// <param name="helper">HTML Helper</param>
        /// <returns>String com tag link de inclusao dos scripts css do plugin</returns>		
        public static MvcHtmlString includeFroalaEditorCSS(this HtmlHelper helper) {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

		    htm.Append(helper.Raw(helper.scripts().scriptFroala));
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/froala_editor.min.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/froala_style.min.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/char_counter.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/code_view.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/colors.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/emoticons.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/file.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/fullscreen.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/image.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/image_manager.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/line_breaker.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/quick_insert.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/table.css")).Append("\" rel=\"stylesheet\" />");
			htm.Append("<link href=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/css/plugins/video.css")).Append("\" rel=\"stylesheet\" />");

			return new MvcHtmlString(htm.ToString());
		}

		/// <summary>
        /// Inclusao de arquivos JS necessarios para utilizacao da editor FROALA
        /// </summary>
        /// <param name="helper">HTML Helper</param>
        /// <returns>String com tag de inclusao dos scripts js do plugin</returns>
		public static MvcHtmlString includeFroalaEditorJS(this HtmlHelper helper) {
			StringBuilder htm = new StringBuilder();
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/froala_editor.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/align.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/char_counter.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/code_beautifier.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/code_view.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/colors.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/emoticons.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/entities.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/file.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/font_family.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/font_size.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/fullscreen.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/image.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/image_manager.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/inline_style.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/line_breaker.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/link.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/lists.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/paragraph_format.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/paragraph_style.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/quick_insert.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/quote.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/table.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/save.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/url.min.js")).Append("\"></script>");
			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/plugins/video.min.js")).Append("\"></script>");

			htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/froala_editor_2.6.0/js/languages/pt_br.js")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}
		
		//
		public static MvcHtmlString includeFroalaEditorJSCustom(this HtmlHelper helper) {
			
			StringBuilder htm = new StringBuilder();
			
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			htm.Append(helper.includeFroalaEditorJS());

			htm.Append("<script src=\"").Append(urlHelper.Content(UtilConfig.linkResourses("js/associatec/froala-custom.js?v=1.0"))).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}

		//Inclusao da biblioteca
		public static MvcHtmlString includeFileApiJS(this HtmlHelper helper) {

			StringBuilder htm = new StringBuilder();

			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/jquery.fileapi/jquery.fileapi.config.js")).Append("\"></script>");
            htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/jquery.fileapi//FileAPI/FileAPI.min.js")).Append("\"></script>");
            htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/jquery.fileapi/FileAPI/FileAPI.exif.js")).Append("\"></script>");
            htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/jquery.fileapi/jquery.fileapi.min.js")).Append("\"></script>");
            htm.Append("<script src=\"").Append(urlHelper.Content("" + UtilConfig.linkResourses() + "js/associatec/plugins/jquery.fileapi/jcrop/jquery.Jcrop.min.js")).Append("\"></script>");

			return new MvcHtmlString(htm.ToString());
		}

	}
}