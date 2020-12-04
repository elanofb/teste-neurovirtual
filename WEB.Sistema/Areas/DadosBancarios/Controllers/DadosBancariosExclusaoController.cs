using System.Web.Mvc;
using BLL.DadosBancarios.Interfaces;
using BLL.DadosBancarios.Services;
using WEB.App_Infrastructure;

namespace WEB.Areas.DadosBancarios.Controllers {
    
    public class DadosBancariosExclusaoController : BaseSistemaController {
        
	    //Atributos
	    private IDadoBancarioExclusaoBL _DadoBancarioExclusaoBL;

	    //Propriedades
	    private IDadoBancarioExclusaoBL ODadoBancarioExclusaoBL => _DadoBancarioExclusaoBL = _DadoBancarioExclusaoBL ?? new DadoBancarioExclusaoBL();
        
	    [HttpPost]
	    public ActionResult excluir(int id) {
		    return Json(this.ODadoBancarioExclusaoBL.excluir(new[] { id }), JsonRequestBehavior.AllowGet);
	    }
        
    }
}