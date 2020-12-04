using System;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using System.Json;
using BLL.Profissoes;

namespace WEB.Areas.Profissoes.Controllers {

    public class ProfissaoExclusaoController : BaseSistemaController {

        //Constantes

        //Atributos
        private IProfissaoExclusaoBL _IProfissaoExclusaoBL;
        
        //Propriedades
        private IProfissaoExclusaoBL OProfissaoExclusaoBL => _IProfissaoExclusaoBL = _IProfissaoExclusaoBL ?? new ProfissaoExclusaoBL();
        
        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            JsonMessage Retorno = new JsonMessage();
            Retorno.error = false;

            foreach(int idExclusao in id) {

                var flagSucesso = this.OProfissaoExclusaoBL.excluir(idExclusao);

                if(!flagSucesso) {
                    Retorno.error = true;
                    Retorno.message = "Algumas exclusões não puderam ser realizadas, tente novamente.";
                }
            }

            Retorno.message = Retorno.message.isEmpty() ? "Os registros foram removidos com sucesso." : Retorno.message;

            return Json(Retorno);
        }

    }
}
