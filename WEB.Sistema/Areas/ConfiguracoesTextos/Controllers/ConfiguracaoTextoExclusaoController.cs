using System.Web.Mvc;
using BLL.ConfiguracoesTextos;
using WEB.App_Infrastructure;

namespace WEB.Areas.ConfiguracoesTextos.Controllers {

    public class ConfiguracaoTextoExclusaoController : BaseSistemaController {

		//Atributos
		private IConfiguracaoTextoExclusaoBL _IConfiguracaoTextoExclusaoBL;

        //Propriedades
	    private IConfiguracaoTextoExclusaoBL OConfiguracaoTextoExclusaoBL => _IConfiguracaoTextoExclusaoBL = _IConfiguracaoTextoExclusaoBL ?? new ConfiguracaoTextoExclusaoBL();

        //
        public JsonResult excluir(string id) {
	        return Json(this.OConfiguracaoTextoExclusaoBL.excluir(id), JsonRequestBehavior.AllowGet);
        }

    }
	
}