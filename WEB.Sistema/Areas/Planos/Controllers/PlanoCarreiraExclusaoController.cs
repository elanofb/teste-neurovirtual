using System.Web.Mvc;
using WEB.App_Infrastructure;
using BLL.Planos;

namespace WEB.Areas.Planos.Controllers {
    
    public class PlanoCarreiraExclusaoController : BaseSistemaController {
                
        //Constantes
        
        //Atributos
        private IPlanoCarreiraExclusaoBL _IPlanoCarreiraExclusaoBL;
        
        //Propriedades
        private IPlanoCarreiraExclusaoBL OPlanoCarreiraExclusaoBL => _IPlanoCarreiraExclusaoBL = _IPlanoCarreiraExclusaoBL ?? new PlanoCarreiraExclusaoBL();
        
        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.OPlanoCarreiraExclusaoBL.excluir(id), JsonRequestBehavior.AllowGet);
        }
        
    }
    
}
