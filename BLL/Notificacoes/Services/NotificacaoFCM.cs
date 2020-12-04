using System;
using System.IO;
using System.Net;
using BLL.Notificacoes.Interface;
using DAL.Notificacoes;
using Newtonsoft.Json;

namespace BLL.Notificacoes.Services {

    public class NotificacaoFCM : IMensageiroNotificacao {

        /// <summary>
        /// Configurar envio de notificacao para firebase cloud messaging
        /// </summary>
        public UtilRetorno enviar(NotificacaoSistema Notificacao) {

            var Retorno = new UtilRetorno();

            string serverKey = "AAAATgqR4Z4:APA91bEbh4nRYC0Rpw92WuxSGQDGikzxMk1yR0_anj_6oBHaJnAdn61ycE7_UpYBQK1elUPVmuG44tPL_fRU54Z4FuPkzfWJFsy9IhCdwvmStSeWrc3AAntLwaMmPFehJhT_yjwudX8b";

            string hostFCM = "https://fcm.googleapis.com/fcm/send";

            try {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(hostFCM);

                httpWebRequest.ContentType = "application/json";

                httpWebRequest.Headers.Add($"Authorization:key={serverKey}");

                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {

                    var objectJson = new { notification = new { body = Notificacao.notificacao, title = Notificacao.titulo } };

                    string json = JsonConvert.SerializeObject(objectJson);

                    streamWriter.Write(json);

                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                var streamResult = httpResponse.GetResponseStream();

                using (var streamReader = new StreamReader(streamResult)) {

                    Retorno.info = streamReader.ReadToEnd();
                }

                Retorno.flagError = false;

            } catch (Exception ex) {

                Retorno.flagError = true;

                Retorno.listaErros.Add($"Não foi possível estabalecer conexão com FCM. Detalhes: {ex.Message}");

                UtilLog.saveError(ex, "Erro conexão FCM");
            }

            return Retorno;
        }
    }
}
