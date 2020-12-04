using System.Web.Mvc;

namespace WEB.Areas.Localizacao.Controllers {

    [AllowAnonymous]
	public class GeoLocalizacaoController : Controller{
        
        [ActionName("modal-localizacao")]
        public ActionResult modalLocalizacao() {

            return View();
        }

    }
}