using System.Web.Mvc;
using BLL.ConfiguracoesTextos;
using WEB.App_Infrastructure;

namespace WEB.Areas.ConfiguracoesTextos.Controllers {

    public class IdiomaExclusaoController : BaseSistemaController {

		//Atributos
		private IIdiomaExclusaoBL _IIdiomaExclusaoBL;

        //Propriedades
	    private IIdiomaExclusaoBL OIdiomaExclusaoBL => _IIdiomaExclusaoBL = _IIdiomaExclusaoBL ?? new IdiomaExclusaoBL();

        //
        public JsonResult excluir(int id) {
	        return Json(this.OIdiomaExclusaoBL.excluir(id), JsonRequestBehavior.AllowGet);
        }

    }
	
}