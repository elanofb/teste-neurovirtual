using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class FluxoCaixaDiarioController : Controller {
        
        public ActionResult index() {

            var ViewModel = new FluxoCaixaDiarioForm {
                tipoBuscaPeriodo = "dtPagamento",
                dtInicioPeriodo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                dtFimPeriodo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month))
            };

            return View(ViewModel);
        }
        
        //
        [ActionName("partial-movimentacao-diaria")]
        public PartialViewResult partialMovimentacaoDiaria(FluxoCaixaDiarioForm Form) {

            var OFluxoCaixaVM = new FluxoCaixaDiarioVM();
            OFluxoCaixaVM.carregarPagamentos(Form);

            var ViewModel = new FluxoCaixaMovimentacaoDiariaVM { listaPagamentos = OFluxoCaixaVM.listaPagamentos };
            ViewModel.agruparDatas(Form);

            return PartialView(ViewModel);
        }

        //
        [ActionName("carregar-evolucao-caixa")]
        public JsonResult carregarEvolucaoCaixa(FluxoCaixaDiarioForm Form) {

            var OFluxoCaixaVM = new FluxoCaixaDiarioVM();
            OFluxoCaixaVM.carregarPagamentos(Form);

            var ViewModel = new FluxoCaixaMovimentacaoDiariaVM();
            ViewModel.listaPagamentos = OFluxoCaixaVM.listaPagamentos;
            ViewModel.agruparDatas(Form);
            
            ViewModel.listaMovimentacaoDiaria = ViewModel.listaMovimentacaoDiaria.OrderBy(x => x.dtReferencia).ToList();

            var ORetorno = new FluxoCaixaEvolucaoDTO();

            ORetorno.datas = ViewModel.listaMovimentacaoDiaria.Select(x => x.dtReferencia.ToString("dd/MM")).ToArray();
            ORetorno.valoresReceitas = ViewModel.listaMovimentacaoDiaria.Select(x => x.valorTotalEntrada).ToArray();
            ORetorno.valoresDespesas = ViewModel.listaMovimentacaoDiaria.Select(x => - x.valorTotalSaida).ToArray();
            ORetorno.saldosAcumulados = ViewModel.listaMovimentacaoDiaria.Select(x => x.saldoAcumulado).ToArray();

            return Json(ORetorno, JsonRequestBehavior.AllowGet);
        }
    }
}