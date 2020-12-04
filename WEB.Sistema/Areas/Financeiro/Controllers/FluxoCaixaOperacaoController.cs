using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {
    
    public class FluxoCaixaOperacaoController : Controller {
        
        // Atributos
        private ITituloReceitaPagamentoBL _ITituloReceitaPagamentoBL;
        private ITituloDespesaPagamentoBL _ITituloDespesaPagamentoBL;

        // Propriedade
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _ITituloReceitaPagamentoBL = _ITituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();
        private ITituloDespesaPagamentoBL OTituloDespesaPagamentoBL => _ITituloDespesaPagamentoBL = _ITituloDespesaPagamentoBL ?? new TituloDespesaPagamentoBL();

        //
        [HttpPost, ActionName("conciliar-pagamentos")]
        public JsonResult conciliarPagamentos(List<FluxoCaixaConciliacaoBancariaDTO> listaPagamentos) {

            var idsReceitas = listaPagamentos.Where(x => x.flagTipoPagamento.Equals("R")).Select(x => x.id).ToArray();

            if (idsReceitas.Any()) {
                this.OTituloReceitaPagamentoBL.conciliarPagamentos(idsReceitas, true);
            }

            var idsDespesas = listaPagamentos.Where(x => x.flagTipoPagamento.Equals("D")).Select(x => x.id).ToArray();

            if (idsDespesas.Any()) {
                this.OTituloDespesaPagamentoBL.conciliarPagamentos(idsDespesas, true);
            }
            
            return Json(true);

        }

        //
        [HttpPost, ActionName("cancelar-conciliacao")]
        public JsonResult cancelarConciliacao(FluxoCaixaConciliacaoBancariaDTO OPagamento) {
            
            if (OPagamento.flagTipoPagamento.Equals("R")) {
                this.OTituloReceitaPagamentoBL.conciliarPagamentos(new [] { OPagamento.id }, false);
            }
            
            if (OPagamento.flagTipoPagamento.Equals("D")) {
                this.OTituloDespesaPagamentoBL.conciliarPagamentos(new [] { OPagamento.id }, false);
            }
            
            return Json(true);

        }

    }
}