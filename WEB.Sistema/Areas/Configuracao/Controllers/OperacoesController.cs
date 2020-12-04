using System.Web.Mvc;
using BLL.Caches;

namespace WEB.Areas.Configuracao.Controllers {

    public class OperacoesController : Controller {

        // 
        public ActionResult index() {


			return View();
        }
        
        // 
        [AllowAnonymous]
		[ActionName("limpar-cache-dados")]
        public ActionResult limparCacheDados() {

			CacheService.getInstance.limparConfiguracoes();

			return Json(new{ error = false, message = "O cache da aplicação foi removido com sucesso!"}, JsonRequestBehavior.AllowGet);
        }
	}
}