using System.IO;
using System.Linq;
using System.Web.Mvc;
using BLL.Notificacoes;
using BLL.Notificacoes.Vendors.Amazon;
using DAL.Notificacoes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WEB.Areas.Notificacoes.Controllers {

    public class PostbackAWSController : Controller {

        //Atributos
        private INotificacaoPostbackCadastroBL _NotificacaoPostbackCadastroBL;

        //Propriedades
        private INotificacaoPostbackCadastroBL ONotificacaoPostbackCadastroBL => this._NotificacaoPostbackCadastroBL = this._NotificacaoPostbackCadastroBL ?? new NotificacaoPostbackCadastroBL();

        //
        [ActionName("post-back"), AllowAnonymous]
        public JsonResult postBack() {

            string json = new StreamReader(Request.InputStream).ReadToEnd();

            json = json.Replace("ses:", "ses_");

            AWSNotification AWSNotification = JsonConvert.DeserializeObject<AWSNotification>(json, new IsoDateTimeConverter());

            foreach (var stringDestino in AWSNotification.Message.mail.destination) {
                
                var ONotificacaoPostback = new NotificacaoPostback();
            
                ONotificacaoPostback.idExternoNotificacao = AWSNotification.Message.mail.messageId;
            
                ONotificacaoPostback.acao = AWSNotification.Message.eventType;
            
                ONotificacaoPostback.contaOrigem = AWSNotification.Message.mail.commonHeaders.from.FirstOrDefault();
            
                ONotificacaoPostback.dtAcao = AWSNotification.Message.mail.timestamp;
            
                ONotificacaoPostback.ipAcao = AWSNotification.Message.mail.tags.sessourceip.FirstOrDefault();
            
                ONotificacaoPostback.meioInteracao = AWSNotification.Message.open?.userAgent;
                
                ONotificacaoPostback.contaDestino = stringDestino;

                ONotificacaoPostbackCadastroBL.salvar(ONotificacaoPostback);
                
            }
            
            return Json(new {});
        }
        
    }
}