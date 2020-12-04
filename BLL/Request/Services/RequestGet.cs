using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace BLL.Request {

	public class RequestGet : IRequestGet {

		//Montar uma requisicao a partir e uma url
		//Disparar o metodos de captura do conteudo
		//Retorno o stream lido
		public string doRequest(string url, NameValueCollection extraHeaders = null) {
			
            var request = (HttpWebRequest)WebRequest.Create(url);
			
			if (extraHeaders != null){
				request.Headers.Add(extraHeaders);    
			}  

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

			return responseString;
		}
	}

}
