using System.Web.Mvc;
using WEB.Areas.PlanoContas.ViewModels;

namespace WEB.Areas.PlanoContas.Controllers {

    public class PlanoContaDREController : Controller {

        // GET: PlanoContas/PlanoContaDRE
        [ActionName("partial-modelo-dre")]
        public ActionResult ModeloDRE() {
            var ViewModel = new PlanoContasDREVM();

            ViewModel.carregarDados();

            return View(ViewModel);
        }

    }

}