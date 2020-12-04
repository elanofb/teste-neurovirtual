using System.Json;
using System.Web.Mvc;
using BLL.Caches;
using BLL.FinanceiroLancamentos;
using WEB.App_Infrastructure;

namespace WEB.Areas.FinanceiroLancamentos.Controllers{

    public class CredorOperacaoController : BaseSistemaController{

        //Atributos
        private ICredorBL _CredorBL;

        //Propriedades
        private ICredorBL OCredorBL => _CredorBL = _CredorBL ?? new CredorBL();
        
        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {

            var ORetorno = this.OCredorBL.alterarStatus(id);
            if (!ORetorno.error) {
                CacheService.getInstance.remover(CredorVWBL.keyCache);
            }

            return Json(ORetorno);

        }

        //
		[HttpPost]
		public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
		    Retorno.error = false;
		    Retorno.message = "Os registros informados foram removidos com sucesso.";

		    foreach (var idItem in id) {

		        var RetornoItem = this.OCredorBL.excluir(idItem);

		        if (RetornoItem.flagError) {
		            Retorno.error = true;
		            Retorno.message = "Algum(ns) registro(s) não pode(ram) ser removido(s).";
		        }
		    }

		    CacheService.getInstance.remover(CredorVWBL.keyCache);

            return Json(Retorno);
		}
        
    }

}