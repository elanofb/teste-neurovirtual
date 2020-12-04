using System;
using System.Web.Mvc;
using BLL.Notificacoes;
using DAL.Notificacoes;
using WEB.Areas.AvisosNotificacoes.ViewModels;
using BLL.AvisosNotificacoes.Services;
using BLL.Notificacoes.Services;

namespace WEB.Areas.AvisosNotificacoes.Controllers {

    [OrganizacaoFilter] 
    public class NotificacaoConsultaController : Controller {

        // Atributos
        private INotificacaoSistemaEnvioBL _INotificacaoSistemaEnvioBL;
        private INotificacaoAssociadoAvulsaBL _INotificacaoAssociadoAvulsaBL;
        

        // Propriedades
        private INotificacaoSistemaConsultaBL NotificacaoConsultaBL { get;  }
        private INotificacaoSistemaEnvioBL ONotificacaoSistemaEnvioBL => _INotificacaoSistemaEnvioBL = _INotificacaoSistemaEnvioBL ?? new NotificacaoSistemaEnvioBL();
        private INotificacaoAssociadoAvulsaBL ONotificacaoAssociadoAvulsaBL => _INotificacaoAssociadoAvulsaBL = _INotificacaoAssociadoAvulsaBL ?? new NotificacaoAssociadoAvulsaBL();

        /// <summary>
        /// 
        /// </summary>
        public NotificacaoConsultaController(INotificacaoSistemaConsultaBL _NotificacaoConsultaBL) {

            this.NotificacaoConsultaBL = _NotificacaoConsultaBL;
        }

        [HttpGet]
        public ActionResult listar() {

            var ViewModel = new AvisoNotificacaoConsultaVM( this.NotificacaoConsultaBL );
            
            ViewModel.carregarInformacoes();

            return View(ViewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult notificacaoFCM() {

            var ONotificacao = new NotificacaoSistema();

            ONotificacao.titulo = $"Título da mensagem: {DateTime.Now}";

            ONotificacao.notificacao = $"Esta é uma mensagem de testes enviada em {DateTime.Now}";

            var Retorno = new NotificacaoFCM().enviar(ONotificacao);

            return Json(Retorno, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ActionName("notificacao-onesignal")]
        public ActionResult notificacaoOneSignal() {

            var ONotificacao = new NotificacaoSistema();

            ONotificacao.titulo = $"Título da mensagem: {DateTime.Now}";

            ONotificacao.notificacao = $"Esta é uma mensagem de testes enviada em {DateTime.Now}";

            var Retorno = new NotificacaoOneSignal().enviar(ONotificacao);

            return Json(Retorno, JsonRequestBehavior.AllowGet);
        }
    }
}