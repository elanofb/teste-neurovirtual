using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BLL.Notificacoes.Vendors.Mailgun {
    
    public class MailgunClient {
        
        private string apiKey { get; set; }
        private readonly string apiVersion = "v3";
        
        private readonly string host = "https://api.mailgun.net";
        private readonly string domainSender = "mailgun.associatec.com.br"; 

        private readonly string sandboxDomain = "sandbox935a02649f2a4ce8aae7ad6a72e8c9a6.mailgun.org";
        private readonly string sandboxApiKey = "key-d653dd0057f8b7158cfe1eef5bfcc3c3";
        
        
        private string urlPath { get; set; }
        private string mediaType { get; set; }
        private HttpClient httpClient;

        /// <summary>
        /// Construtor
        /// </summary>
        public MailgunClient(string apiKeyparam, bool flagSandbox = false) {

            this.apiKey = apiKeyparam;

            if (flagSandbox) {

                this.domainSender = this.sandboxDomain;

                this.apiKey = sandboxApiKey;

            }
            
            this.httpClient = new HttpClient();
            
            this.configurarConexao();
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void configurarConexao() {

            this.httpClient.BaseAddress = new Uri(this.host);
            
            Dictionary<string, string> headers = new Dictionary<string, string>{
                { "Authorization", "api:" + this.apiKey },
                { "Content-Type", "application/json" },
                { "User-Agent", "mailgun/1.0.0 csharp" },
                { "Accept", "application/json" }
            };
            
            foreach (var header in headers){
                
                if (header.Key == "Authorization"){
                
                    var userPass = $"api:{this.apiKey}";

                    var byteArray = Encoding.ASCII.GetBytes(userPass);
                        
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    
                }else if (header.Key == "Content-Type"){
                    
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.Value));
                    
                    this.mediaType = header.Value;
                    
                }else{
                    
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }            
        }
        
        /// <summary>
        /// 
        /// </summary>
        private string construirUrl(string urlPathParam, string queryParams = null) {

            this.urlPath = $"{apiVersion}/{domainSender}/{urlPathParam}";

            if (queryParams.isEmpty()) {

                return this.urlPath;
            }

            var listaParam = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(queryParams);

            string query = "?";
            string url = this.urlPath;
            
            foreach (var pair in listaParam){
                
                if (query != "?"){
                    
                    query = query + "&";
                }

                query = query + pair.Key + "=" + pair.Value;
            }

            url = url + query;

            return url;
        }        
        
        /// <summary>
        /// Realizar conexao e devolver retorno HTTP
        /// </summary>
        public async Task<HttpResponseMessage> enviarAsync(HttpRequestMessage request){
            
            HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            
           return response;
        }        
        
        /// <summary>
        /// Enviar requisicao
        /// </summary>
        public async Task<HttpResponseMessage> requestAsync(string urlPath, string method = "GET", string requestBody = null, string queryParams = null){
            
            var endpoint = httpClient.BaseAddress + this.construirUrl(urlPath, queryParams);

            // Build the request body
            StringContent content = null;
            
            if (requestBody != null){
                
                content = new StringContent(requestBody, Encoding.UTF8, this.mediaType);
            }

            // Build the final request
            var request = new HttpRequestMessage{
                
                Method = new HttpMethod(method),
                
                RequestUri = new Uri(endpoint),
                
                Content = content
            };
            
            return await enviarAsync(request).ConfigureAwait(false);
        }
    }
}
