using System.Web.Mvc;
using WEB.Areas.FinanceiroLancamentos.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.FinanceiroLancamentos.Controllers {

    [OrganizacaoFilter]
    public class ExtratoPeriodoController : Controller {

        public ActionResult listar(ExtratoPorPeriodoVM ViewModel) {
            
            ViewModel.carregarInformacoes();
            
            if (ViewModel.flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OExtratoPorPeriodoExportacao = new ExtratoPorPeriodoExportacao();

                OExtratoPorPeriodoExportacao.baixarExcel(ViewModel);

                return null;
            }
            
            return View(ViewModel);
        }
    }
}