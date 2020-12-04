using System.Text;
using System.Web.Mvc;

namespace WEB.Areas.Financeiro.Helpers {

	public static class TituloDespesaPagamentoExtensions {

		public static MvcHtmlString acoesDespesaPagamento(this HtmlHelper helper, int idRegistro, string flagPago = "N", string extraClasses = "", string pagina = "listar") {

			var htm = new StringBuilder();			
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
		    var title = "Registrar Pagamento";		    
            var function = "onclick=\"APagar.modelRegistrarPagamento(this, 'listar');\"";
			var url = urlHelper.Action(null, null, new {id = idRegistro});

            if (flagPago == "S")
		    {
			    url = urlHelper.Action(null, null, new {id = idRegistro});
		        title = "Cancelar Pagamento";
                function = "onclick=\"APagar.modelCancelarPagamento(this, '"+pagina+"');\"";
		    }

            htm.Append("<a href='javascript:void(0);' "+ function +" class='box-acoes "+ extraClasses + "' data-id='" + idRegistro + "' data-url='" + url + "' data-toggle='tooltip' title='"+title+"'>");
		    if (flagPago == "N"){
		        htm.Append("<i class=\"fa fa-check-circle\"></i>");
		    }else{
		        htm.Append("<i class=\"fa fa-times\"></i>");
		    }
			htm.Append("</a>");

			return new MvcHtmlString(htm.ToString());
		}

	}
}