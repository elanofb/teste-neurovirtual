using System.Web.Mvc;
using BLL.Notificacoes.Vendors.SendGrid;
using WEB.App_Infrastructure;

namespace WEB.Areas.Notificacoes.Controllers {

	public class SendGridTesteController: BaseSistemaController {

		//Atributos
		private string apiKey = "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";

		//Propriedades

		[ActionName("carregar-blocks")]
		public ActionResult carregarBlocks() {
			
			var client = new SendGridClient(apiKey);

			string queryParams = @"{
              //'end_time': 1443651154, 
              'limit': 10, 
              'offset': 0 
              //'start_time': 1443651141
            }";
            
			var response = client.requestAsync("suppression/blocks", "GET", queryParams: queryParams).Result;

			var data = new {
				response.Content,
				response.IsSuccessStatusCode,
				response.ReasonPhrase,
				response.StatusCode,
				response.Version,
				body = response.Content.ReadAsStringAsync().Result
			};

			return Json(data, JsonRequestBehavior.AllowGet);
		}


		[ActionName("carregar-bounces")]
		public ActionResult carregarBounces() {
			
			var client = new SendGridClient(apiKey);

			string queryParams = @"{
              //'end_time': 1443651154, 
              'limit': 10, 
              'offset': 0 
              //'start_time': 1443651141
            }";
            
			var response = client.requestAsync("suppression/bounces", "GET", queryParams: queryParams).Result;

			var data = new {
				response.Content,
				response.IsSuccessStatusCode,
				response.ReasonPhrase,
				response.StatusCode,
				response.Version,
				body = response.Content.ReadAsStringAsync().Result
			};

			return Json(data, JsonRequestBehavior.AllowGet);
		}		

		[ActionName("carregar-emails-invalidos")]
		public ActionResult carregarEmailsInvalidos() {
			
			var client = new SendGridClient(apiKey);

			string queryParams = @"{
              //'end_time': 1443651154, 
              'limit': 10, 
              'offset': 0 
              //'start_time': 1443651141
            }";
            
			var response = client.requestAsync("suppression/invalid_emails", "GET", queryParams: queryParams).Result;

			var data = new {
				response.Content,
				response.IsSuccessStatusCode,
				response.ReasonPhrase,
				response.StatusCode,
				response.Version,
				body = response.Content.ReadAsStringAsync().Result
			};

			return Json(data, JsonRequestBehavior.AllowGet);
		}		
		

		[ActionName("carregar-spam-reports")]
		public ActionResult carregarSpamReports() {
			
			var client = new SendGridClient(apiKey);

			string queryParams = @"{
              //'end_time': 1443651154, 
              'limit': 10, 
              'offset': 0 
              //'start_time': 1443651141
            }";
            
			var response = client.requestAsync("suppression/spam_reports", "GET", queryParams: queryParams).Result;

			var data = new {
				response.Content,
				response.IsSuccessStatusCode,
				response.ReasonPhrase,
				response.StatusCode,
				response.Version,
				body = response.Content.ReadAsStringAsync().Result
			};

			return Json(data, JsonRequestBehavior.AllowGet);
		}			
	}
}