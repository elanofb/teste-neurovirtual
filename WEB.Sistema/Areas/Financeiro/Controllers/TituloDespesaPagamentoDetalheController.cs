using System;
using System.Web.Mvc;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class TituloDespesaPagamentoDetalheController : Controller {

        //Carrega o detalhe do pagamento do titulo
        [HttpGet, ActionName("modal-detalhe-pagamento")]
        public ActionResult modalDetalhePagamento(int? id) {

            var ViewModel = new PagamentoDespesaDetalheVM();

            ViewModel.idTituloDespesaPagamento = id.toInt();
            
            ViewModel.flagEdicao = false;
            ViewModel.carregar();

            if (!(ViewModel.OPagamentoDespesa.id > 0)) {
                return Json(new { flagErro = true, message = "Não foi possível encontrar o titulo." }, JsonRequestBehavior.AllowGet);
            }
            
            return PartialView(ViewModel);
        }
        
    }

}
