using System.Web.Mvc;
using WEB.Areas.PlanoContas.ViewModels;

namespace WEB.Areas.PlanoContas.Controllers{
    
    public class PlanoContaController : Controller{
        

        // GET: PlanoContas/PlanoConta
        public ActionResult Index() {

            var ViewModel = new PlanoContasVM();

            ViewModel.carregarDados();

            return View(ViewModel);
        }
    }
}