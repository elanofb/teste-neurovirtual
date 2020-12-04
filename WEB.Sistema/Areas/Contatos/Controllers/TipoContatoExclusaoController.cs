using BLL.Contatos;
using System.Web.Mvc;
using WEB.App_Infrastructure;

namespace WEB.Areas.Contatos.Controllers {
    
    [OrganizacaoFilter]
    public class TipoContatoExclusaoController : BaseSistemaController {
        
        // Atributos
        private ITipoContatoExclusaoBL _ITipoContatoExclusaoBL;

        // Propriedades
        private ITipoContatoExclusaoBL OTipoContatoExclusaoBL => _ITipoContatoExclusaoBL = _ITipoContatoExclusaoBL ?? new TipoContatoExclusaoBL();

        //
        public JsonResult excluir(int[] id) {
            return Json(this.OTipoContatoExclusaoBL.excluir(id), JsonRequestBehavior.AllowGet);
        }
        
    }
    
}