using System;
using System.Web.Mvc;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

    public class ResultadoFinanceiroController : Controller {
        
        public ActionResult index() {

            var ViewModel = new ResultadoFinanceiroForm();

            ViewModel.tipoBuscaPeriodo = "dtPagamento";
            
            return View(ViewModel);

        }

        [HttpPost, ActionName("partial-resultados")]
        public PartialViewResult partialResultados(ResultadoFinanceiroForm Form) {

            if (!Form.dtInicioPeriodo.HasValue || !Form.dtFimPeriodo.HasValue) {

                Form.dtInicioPeriodo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                Form.dtFimPeriodo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

            }
            
            var OResultadoFinanceiroVM = new ResultadoFinanceiroVM();

            OResultadoFinanceiroVM.carregarPagamentos(Form);

            var ViewModel = new ResultadoFinanceiroTotalizadoresVM();
            ViewModel.calcularTotais(OResultadoFinanceiroVM);

            return PartialView(ViewModel);

        }
        
    }
}