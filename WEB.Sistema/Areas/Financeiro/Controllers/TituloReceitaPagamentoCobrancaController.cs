using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;

namespace WEB.Areas.Financeiro.Controllers {
    public class TituloReceitaPagamentoCobrancaController : Controller {

        //Atributos
        private ITituloReceitaPagamentoCobrancaBL _TituloReceitaPagamentoCobrancaBL;

        //Propriedades
        private ITituloReceitaPagamentoCobrancaBL OTituloReceitaPagamentoBL => (_TituloReceitaPagamentoCobrancaBL = _TituloReceitaPagamentoCobrancaBL ?? new TituloReceitaPagamentoCobrancaBL());

		//Abrir o modal com formulario para registro da data de pagamento
		[ActionName("enviar-email-cobranca")]
        public ActionResult enviarEmailCobranca(int id) {

		    var Retorno = OTituloReceitaPagamentoBL.enviarEmailCobranca(id);

		    return Json(new {error = Retorno.flagError, message = Retorno.listaErros.FirstOrDefault()}, JsonRequestBehavior.AllowGet);
        }


    }
}