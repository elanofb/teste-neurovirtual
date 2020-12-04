using System;
using System.IO;
using System.Net;
using System.Text;
using BLL.Notificacoes.Interface;
using DAL.Notificacoes;
using Newtonsoft.Json;

namespace BLL.Notificacoes.Services {

    public class NotificacaoOneSignal : IMensageiroNotificacao {

        /// <summary>
        /// Configurar envio de notificacao para firebase cloud messaging
        /// </summary>
        public UtilRetorno enviar(NotificacaoSistema Notificacao) {

            var Retorno = new UtilRetorno();

            string host = "https://onesignal.com/api/v1/notifications";

            string appId = "ca6e04fb-99b7-47aa-8283-7b421530d386";

            string apiKey = "Yjk0ZDhhYmYtNThjNy00NGNmLTgwNWMtMmM4M2IyYjM3NzU4";


            var request = WebRequest.Create(host) as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", $"Basic {apiKey}");

            var obj = new {
                app_id = appId,
                contents = new { en = Notificacao.notificacao },
                included_segments = new string[] { "All" }
            };

            var param = JsonConvert.SerializeObject(obj);

            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try {
                using (var writer = request.GetRequestStream()) {

                    writer.Write(byteArray, 0, byteArray.Length);

                }

                using (var response = request.GetResponse() as HttpWebResponse) {

                    using (var reader = new StreamReader(response.GetResponseStream())) {

                        responseContent = reader.ReadToEnd();

                    }
                }

                Retorno.info = responseContent;

            } catch (WebException ex) {

                Retorno.flagError = true;

                Retorno.listaErros.Add($"Não foi possível estabalecer conexão com OneSignal. Detalhes: {ex.Message}");

                UtilLog.saveError(ex, "Erro conexão OneSignal");

                System.Diagnostics.Debug.WriteLine(ex.Message);

                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());

            } catch (Exception ex) {

                Retorno.flagError = true;

                Retorno.listaErros.Add($"Não foi possível estabalecer conexão com OneSignal. Detalhes: {ex.Message}");

                UtilLog.saveError(ex, "Erro conexão OneSignal");
            }

            return Retorno;
        }
    }
}
