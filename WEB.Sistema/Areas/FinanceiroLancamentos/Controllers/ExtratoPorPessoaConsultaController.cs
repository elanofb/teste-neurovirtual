using System.Web.Mvc;
using WEB.Areas.LancamentoRecebimentos.ViewModels;

namespace WEB.Areas.FinanceiroLancamentos.Controllers {

    [OrganizacaoFilter]
    public class ExtratoPorPessoaConsultaController : Controller {

        public ActionResult listar(ExtratoConsultaVM ViewModel) {

            ViewModel.carregarInformacoes();

            return View(ViewModel);
            
        }

    }

}