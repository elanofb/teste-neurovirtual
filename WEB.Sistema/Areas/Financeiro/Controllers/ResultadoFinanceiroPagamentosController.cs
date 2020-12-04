using System;
using System.Web.Mvc;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

    public class ResultadoFinanceiroPagamentosController : Controller {
        
        [ActionName("partial-carregar-pagamentos")]
        public PartialViewResult partialCarregarPagamentos(ResultadoFinanceiroForm Form) {

            if (!Form.dtInicioPeriodo.HasValue || !Form.dtFimPeriodo.HasValue) {

                Form.dtInicioPeriodo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                Form.dtFimPeriodo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

            }
            
            var ViewModel = new ResultadoFinanceiroVM();
            ViewModel.carregarPagamentos(Form);

            if (Form.tipoResultado.isEmpty()) {
                return PartialView("partial-pagamentos-detalhado", ViewModel);
            }

            var ViewModelAgrupada = new ResultadoFinanceiroAgrupadoVM(ViewModel.listaPagamentos);
            
            if (Form.tipoResultado.Equals("tipo")) {
                ViewModelAgrupada.agruparPorTipo();
                return PartialView("partial-pagamentos-agrupados", ViewModelAgrupada);
            }

            if (Form.tipoResultado.Equals("cc")) {
                ViewModelAgrupada.agruparPorCentroCusto();
                return PartialView("partial-pagamentos-agrupados", ViewModelAgrupada);
            }

            if (Form.tipoResultado.Equals("mc")) {
                ViewModelAgrupada.agruparPorMacroConta();
                return PartialView("partial-pagamentos-agrupados", ViewModelAgrupada);
            }

            return PartialView("partial-pagamentos-detalhado", ViewModel);

        }
        
    }
}