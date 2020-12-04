using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text;

namespace BLL.Request {

    public class RequestPost : IRequestPost{

        //Montar uma requisicao a partir e uma url
        //Disparar o metodos de captura do conteudo
        //Retorno o stream lido
        public string postRequest(string url, byte[] data, NameValueCollection extraHeaders = null) {
            
            WebRequest ORequest = WebRequest.Create(url);
            
            ORequest.Method = "POST";
            
            ORequest.ContentType = "application/x-www-form-urlencoded";
                
            ORequest.ContentLength = data.Length;                        
                            
            if (extraHeaders != null){
                ORequest.Headers.Add(extraHeaders);    
            }           
            
            using (Stream stream = ORequest.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }
            
            var httpResponse = (HttpWebResponse)ORequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                return streamReader.ReadToEnd();
            }
        }
    }

}
