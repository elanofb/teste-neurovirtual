using System;
using System.Web.Mvc;
using BLL.Notificacoes;
using System.Json;

namespace WEB.Areas.AvisosNotificacoes.Controllers {

    public class AvisoNotificacaoOperacaoController : Controller {

        // Atributos
        private INotificacaoSistemaEnvioBL _INotificacaoSistemaEnvioBL;

        // Propriedades
        private INotificacaoSistemaEnvioBL ONotificacaoSistemaEnvioBL => _INotificacaoSistemaEnvioBL = _INotificacaoSistemaEnvioBL ?? new NotificacaoSistemaEnvioBL();

        public AvisoNotificacaoOperacaoController() {

        }
        
        [HttpPost, ActionName("excluir-envio")]
		public ActionResult excluirEnvio() {

            var id = UtilRequest.getInt32("id");

			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			UtilRetorno RetornoExclusao = this.ONotificacaoSistemaEnvioBL.excluir(id);
				
			if (RetornoExclusao.flagError) { 
				Retorno.error = true;
				Retorno.message = "Algumas exclusões não puderam ser realizadas, tente novamente.";
			}

            Retorno.message = Retorno.message ?? "O envio foi excluído com sucesso.";

            return Json(Retorno, JsonRequestBehavior.AllowGet);
		}
        
    }
}