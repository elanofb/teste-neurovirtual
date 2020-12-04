using System.Web.Mvc;
using BLL.Relacionamentos;
using WEB.App_Infrastructure;

namespace WEB.Areas.Relacionamentos.Controllers {
    
    [OrganizacaoFilter]
    public class OcorrenciaRelacionamentoExclusaoController : BaseSistemaController {
        
        // Atributos
        private IOcorrenciaRelacionamentoExclusaoBL _IOcorrenciaRelacionamentoExclusaoBL;

        // Propriedades
        private IOcorrenciaRelacionamentoExclusaoBL OOcorrenciaRelacionamentoExclusaoBL => _IOcorrenciaRelacionamentoExclusaoBL = _IOcorrenciaRelacionamentoExclusaoBL ?? new OcorrenciaRelacionamentoExclusaoBL();

        //
        public JsonResult excluir(int[] id) {
            return Json(this.OOcorrenciaRelacionamentoExclusaoBL.excluir(id), JsonRequestBehavior.AllowGet);
        }
        
    }
    
}