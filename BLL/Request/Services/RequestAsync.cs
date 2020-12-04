using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text;

namespace BLL.Request {

    public class RequestAsync : IRequestAsync {

        //Receber uma requisicao montada
        //Criar uma tarefa assincrona para realizar a requisicao
        //Capturar e devolver o conteudo remoto
        public async Task<TextReader> doRequestAsync(WebRequest OWebRequest) {
            var ConnectTask = Task.Factory.FromAsync(
                                            (cb, objeto) =>
                                                            ((HttpWebRequest)objeto).BeginGetResponse(cb, objeto),
                                                            res => ((HttpWebRequest)res.AsyncState).EndGetResponse(res),
                                                            OWebRequest
                                            );
            var resultado = await ConnectTask;
            var resp = resultado;
            var stream = resp.GetResponseStream();
            var sr = new StreamReader(stream);
            return sr;
        }

        //Montar uma requisicao a partir e uma url
        //Disparar o metodos de captura do conteudo
        //Retorno o stream lido
        public async Task<TextReader> doRequestAsync(string url) {

            HttpWebRequest ORequest = HttpWebRequest.CreateHttp(url);

            //ORequest.AllowReadStreamBuffering = true;
            var retorno = await this.doRequestAsync(ORequest);

            return retorno;
        }

        //Montar uma requisicao a partir e uma url
        //Disparar o metodos de captura do conteudo
        //Retorno o stream lido
        public async Task<TextReader> postRequestAsync(string url, byte[] data) {

            WebRequest ORequest = WebRequest.Create(url);

            ORequest.Method = "POST";

            ORequest.ContentType = "application/x-www-form-urlencoded";

            ORequest.ContentLength = data.Length;

            using (Stream stream = ORequest.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }

            //ORequest.AllowReadStreamBuffering = true;
            var retorno = await this.doRequestAsync(ORequest);

            return retorno;
        }
    }

}
