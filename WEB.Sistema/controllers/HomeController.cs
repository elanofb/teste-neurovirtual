using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using WEB.App_Infrastructure;

namespace WEB.Controllers {

    public class HomeController : BaseSistemaController {
		

		//Atributos

		//Propriedades

		//Página inicial após o usuário estar logado no sistema
        public ActionResult Index() {


            return View();
        }


	    /// <summary>
	    /// 
	    /// </summary>
	    private static object GetResponse() {
		    
		    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		    
		    HttpWebRequest request = WebRequest.CreateHttp("https://tls12.pagar.me");
		    
		    request.UserAgent = "pagarme-net/tls-example";
		    
		    request.Method = "GET";
		    
		    try {

			    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			    
			    Stream dataStream = response.GetResponseStream();
			    
			    StreamReader reader = new StreamReader(dataStream);
			    
			    return reader.ReadToEnd();
			    
		    } catch(Exception e) {
			    
			    return e;
		    }
		    
	    }

    }
	
}