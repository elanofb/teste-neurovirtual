using System.Linq;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.Profissoes.ViewModels;

namespace WEB.Areas.Profissoes.Controllers {

    public class ProfissaoConsultaController : BaseSistemaController {

        //
        [HttpGet]
        public ActionResult index() {
            
            var ViewModel = new ProfissaoConsultaVM();
            
            ViewModel.montarLista();
            
            ViewModel.carregar();

            return View(ViewModel);
        }
        
        /// <summary>
        /// Listar ajax das contas bancárias
        /// </summary>
        [ActionName("listar-ajax")]
        public ActionResult listarAjax() {

            var ViewModel = new ProfissaoConsultaVM();
            
            ViewModel.montarLista();
            
            var query = ViewModel.listaProfissoes
                                 .Select(x => new { value = x.id, text = x.descricao })
                                 .OrderBy(x => x.text);

            var lista = query.ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

    }
}
