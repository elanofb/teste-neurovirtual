using System.Web.Mvc;
using WEB.App_Infrastructure;
using BLL.Representantes;

namespace WEB.Areas.Representantes.Controllers {

    public class RepresentanteExclusaoController : BaseSistemaController {

        //Constantes

        //Atributos
        private IRepresentanteExclusaoBL _IRepresentanteExclusaoBL;

        //Propriedades
        private IRepresentanteExclusaoBL ORepresentanteExclusaoBL => _IRepresentanteExclusaoBL = _IRepresentanteExclusaoBL ?? new RepresentanteExclusaoBL();
        
        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.ORepresentanteExclusaoBL.excluir(id), JsonRequestBehavior.AllowGet);
        }

    }
    
}
