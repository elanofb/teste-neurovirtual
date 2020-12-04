using System;
using System.Web.Mvc;
using BLL.Notificacoes;
using System.Json;
using BLL.AvisosNotificacoes.Services;

namespace WEB.Areas.AvisosNotificacoes.Controllers {

    [OrganizacaoFilter] 
    public class NotificacaoExclusaoController : Controller {

        // Atributos
        private INotificacaoSistemaExclusaoBL _NotificacaoSistemaExclusaoBL;
        private INotificacaoSistemaEnvioBL _INotificacaoSistemaEnvioBL;
        private INotificacaoAssociadoAvulsaBL _INotificacaoAssociadoAvulsaBL;

        // Propriedades
        private INotificacaoSistemaExclusaoBL ONotificacaoSistemaExclusaoBL => _NotificacaoSistemaExclusaoBL = _NotificacaoSistemaExclusaoBL ?? new NotificacaoSistemaExclusaoBL();
        private INotificacaoSistemaEnvioBL ONotificacaoSistemaEnvioBL => _INotificacaoSistemaEnvioBL = _INotificacaoSistemaEnvioBL ?? new NotificacaoSistemaEnvioBL();
        private INotificacaoAssociadoAvulsaBL ONotificacaoAssociadoAvulsaBL => _INotificacaoAssociadoAvulsaBL = _INotificacaoAssociadoAvulsaBL ?? new NotificacaoAssociadoAvulsaBL();

        public NotificacaoExclusaoController() {
        }

        [HttpPost]
        public ActionResult excluir(int[] id) {
            
            JsonMessage Retorno = new JsonMessage();
            
            Retorno.error = false;

            foreach (int idExclusao in id) {
                
                UtilRetorno RetornoExclusao = this.ONotificacaoSistemaExclusaoBL.excluir(idExclusao);

                if (RetornoExclusao.flagError) {
                    
                    Retorno.error = true;
                    
                    Retorno.message = "Algumas exclusões não puderam ser realizadas, tente novamente.";
                    
                }
                
            }

            return Json(Retorno, JsonRequestBehavior.AllowGet);
        }

    }
}