using System;
using System.Web.Mvc;
using BLL.ConfiguracoesAssociados;

namespace WEB.Areas.ConfiguracoesAssociados.Controllers {
    public class ConfiguracaoAssociadoCamposExclusaoController : Controller {

        //Atributos
        private IConfiguracaoAssociadoCampoExclusaoBL _ConfiguracaoAssociadoCampoExclusaoBL;

        //Services
        private IConfiguracaoAssociadoCampoExclusaoBL OConfiguracaoAssociadoCampoExclusaoBL => _ConfiguracaoAssociadoCampoExclusaoBL = _ConfiguracaoAssociadoCampoExclusaoBL ?? new ConfiguracaoAssociadoCampoExclusaoBL();

        /// 
        [HttpPost, ActionName("excluir")]
        public ActionResult excluir(int? id) {

            var Retorno = this.OConfiguracaoAssociadoCampoExclusaoBL.excluir(id.toInt());

            return Json(Retorno);
        }
    }
}