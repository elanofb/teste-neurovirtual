using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace System {

    public class UtilHTTP {

        //
        public UtilHTTP() {

        }

        public static async Task<string> post(string urlPost, string dados) {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(urlPost);
            try {
                req.Method = "POST";
                req.Timeout = 300000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.UTF8.GetBytes(dados);
                req.ContentLength = sentData.Length;
                using (System.IO.Stream sendStream = req.GetRequestStream()) {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();
                }
                WebResponse res = await req.GetResponseAsync();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8)) {
                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);

                    while (count > 0) {
                        String str = new String(read, 0, count);
                        Out += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
            } catch (ArgumentException ex) {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            } catch (WebException ex) {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            } catch (Exception ex) {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }

        public static string postSync(string urlPost, string dados) {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(urlPost);
            try {
                req.Method = "POST";
                req.Timeout = 300000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.UTF8.GetBytes(dados);
                req.ContentLength = sentData.Length;
                using (System.IO.Stream sendStream = req.GetRequestStream()) {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();
                }
                WebResponse res = req.GetResponse();
                IO.Stream ReceiveStream = res.GetResponseStream();
                using (IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8)) {
                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);

                    while (count > 0) {
                        String str = new String(read, 0, count);
                        Out += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
            } catch (ArgumentException ex) {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            } catch (WebException ex) {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            } catch (Exception ex) {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }


        //
        public async Task<string> postWithFile(string urlPost, string dados, List<FileInfo> listFiles) {

            using (var client = new HttpClient()) {

                client.BaseAddress = new Uri(urlPost);

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                MultipartFormDataContent content = new MultipartFormDataContent();

                foreach (var OFile in listFiles) {

                    string filepath = OFile.FullName;

                    string filename = OFile.Name;
                    
                    ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(filepath));

                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = filename };

                    content.Add(fileContent);

                }

                HttpResponseMessage response = await client.PostAsync(dados, content);

                string returnString = await response.Content.ReadAsStringAsync();

                return returnString;
            }
        }

        /// <summary>
        /// Sending GET request.
        /// </summary>
        /// <param name="urlGet">Request Url.</param>
        /// <param name="dados">Data for request.</param>
        /// <returns>Response body.</returns>
        public static string get(string urlGet, string dados) {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(urlGet + (string.IsNullOrEmpty(dados) ? "" : "?" + dados));
            try {
                System.Net.WebResponse resp = req.GetResponse();
                using (System.IO.Stream stream = resp.GetResponseStream()) {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream)) {
                        Out = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            } catch (ArgumentException ex) {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            } catch (WebException ex) {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            } catch (Exception ex) {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }
    }
}
