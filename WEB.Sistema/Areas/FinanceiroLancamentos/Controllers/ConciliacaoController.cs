using System.Web.Mvc;
using WEB.Areas.FinanceiroLancamentos.ViewModels;

namespace WEB.Areas.FinanceiroLancamentos.Controllers {

    [OrganizacaoFilter]
    public class ConciliacaoController : Controller {

        public ActionResult index(){
    
            var ViewModel = new ConciliacaoVM(); 
            ViewModel.carregarPeriodo();
            
            return View(ViewModel);
        }
        
        //
        [HttpPost, ActionName("partial-lancamentos")]
        public ActionResult partialLancamentos(ConciliacaoVM ViewModel){
        
            ViewModel.carregarPeriodo();
        
            ViewModel.carregarLancamentos();
            
            return PartialView(ViewModel);
        }

        //
        [HttpPost, ActionName("partial-conciliacoes")]
        public ActionResult partialConciliacoes(ConciliacaoVM ViewModel)
        {
            ViewModel.carregarConciliacoes();
            
            return PartialView(ViewModel);
        }
    }
}