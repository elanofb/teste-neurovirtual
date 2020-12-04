using System.Web.Mvc;
using BLL.ConfiguracoesTextos;
using WEB.App_Infrastructure;

namespace WEB.Areas.ConfiguracoesTextos.Controllers {

    public class ConfiguracaoLabelExclusaoController : BaseSistemaController {

		//Atributos
		private IConfiguracaoLabelExclusaoBL _IConfiguracaoLabelExclusaoBL;

        //Propriedades
	    private IConfiguracaoLabelExclusaoBL OConfiguracaoLabelExclusaoBL => _IConfiguracaoLabelExclusaoBL = _IConfiguracaoLabelExclusaoBL ?? new ConfiguracaoLabelExclusaoBL();

        //
        public JsonResult excluir(string id) {
	        return Json(this.OConfiguracaoLabelExclusaoBL.excluir(id), JsonRequestBehavior.AllowGet);
        }

    }
	
}