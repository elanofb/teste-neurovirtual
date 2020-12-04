using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Erros;
using DAL.Erros;

namespace WEB.Areas.Erros.Controllers{
    [AllowAnonymous]
    public class ErroController : Controller{

        protected LogErroBL _LogErroBL = new LogErroBL();

        //
        public ActionResult Index(){
            return View("error500");
        }

        //
        [ActionName("login-expirado")]
        public ActionResult loginExpirado(){

            return View();
        }

        //
        public ActionResult error401(){
            Response.StatusCode = 401;
            Response.TrySkipIisCustomErrors = true;
            
            if (UtilRequest.getString("aspxerrorpath").IndexOf("AreaAssociados", StringComparison.Ordinal) != -1) {
                return RedirectToAction("error401", "Erro", new { area = "AreaAssociados/Erros" });    
            }
            
            return View();
        }

        //
        public ActionResult error403(){
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;
            
            if (UtilRequest.getString("aspxerrorpath").IndexOf("AreaAssociados", StringComparison.Ordinal) != -1) {
                return RedirectToAction("error403", "Erro", new { area = "AreaAssociados/Erros" });    
            }
            
            return View();
        }

        //
        public ActionResult error404(){
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            if (UtilRequest.getString("aspxerrorpath").IndexOf("AreaAssociados", StringComparison.Ordinal) != -1) {
                return RedirectToAction("error404", "Erro", new { area = "AreaAssociados/Erros" });    
            }
            
            return View();
        }

        //
        public ActionResult error500(){
			LogErro Erro = new LogErro();

	        try {
				
	            Response.StatusCode = 500;
	            
	            Response.TrySkipIisCustomErrors = true;
	            
				Erro = this._LogErroBL.listar()
                                    .OrderByDescending(x => x.id)
	                                .FirstOrDefault();

	            Erro = Erro ?? new LogErro();
	            
	        } catch (Exception ex) {
		        Erro.exceptionMessage = ex.Message;
		        Erro.exceptionTrace = ex.StackTrace;
	        }

            if (UtilRequest.getString("aspxerrorpath").IndexOf("AreaAssociados", StringComparison.Ordinal) != -1) {
                return RedirectToAction("error500", "Erro", new { area = "AreaAssociados/Erros" });    
            }
            
            return View(Erro);
        }
            
		//
        [ActionName("sem-organizacao")]
		public ActionResult semOrganizacao() {

			Response.StatusCode = 401;

            Response.TrySkipIisCustomErrors = true;

            return View();
		}
               
    }
}
