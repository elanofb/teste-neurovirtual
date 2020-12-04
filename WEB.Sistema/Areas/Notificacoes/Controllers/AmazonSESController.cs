using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using WEB.App_Infrastructure;

namespace WEB.Areas.Notificacoes.Controllers {

    public class AmazonSESController : BaseSistemaController {

        //Atributos
		private BasicAWSCredentials credentials = new BasicAWSCredentials("AKIAIHEQU66AM2VFLIKQ", "W1m6S8SyGBTHxQF0hiO+hTjM8gch8ysZ+S8Z6dEB");

        //Propriedades

		[ActionName("enviar-email")]
        public ActionResult enviarEmail() {

            // Replace sender@example.com with your "From" address.
            // This address must be verified with Amazon SES.
            string senderAddress = "suporte@sinclog.com.br";

            // Replace recipient@example.com with a "To" address. If your account
            // is still in the sandbox, this address must be verified.
            string receiverAddress1 = "selecao@sinctec.com.br";
			string receiverAddress2 = "jeihson@sinctec.com.br";
			string receiverAddress3 = "priscila.soares@sinctec.com.br";

            // The configuration set to use for this email. If you do not want to use a
            // configuration set, comment out the following property and the
            // ConfigurationSetName = configSet argument below. 
            //string configSet = "ConfigSet";			

            // The subject line for the email.
            string subject = "Amazon SES test (AWS SDK for .NET)";

            // The email body for recipients with non-HTML email clients.
            string textBody = "Amazon SES Test (.NET)\r\n"
                              + "This email was sent through Amazon SES "
                              + "using the AWS SDK for .NET.";

            // The HTML body of the email.
            string htmlBody = @"<html>
					<head></head>
					<body>
					  <h1>Amazon SES Test (AWS SDK for .NET)</h1>
					  <p>This email was sent with
						<a href='https://aws.amazon.com/ses/'>Amazon SES</a> using the
						<a href='https://aws.amazon.com/sdk-for-net/'>
						  AWS SDK for .NET</a>.</p>
					</body>
					</html>";
			
            using (var client = new AmazonSimpleEmailServiceClient(credentials, RegionEndpoint.USEast1)) {
				
				
				
                var sendRequest = new SendEmailRequest {
                                                           Source = senderAddress,
                                                           Destination = new Destination {
                                                                                             ToAddresses = new List<string> {receiverAddress1, receiverAddress2, receiverAddress3}
                                                                                         },
                                                           Message = new Message {
                                                                                     Subject = new Content(subject),
                                                                                     Body = new Body {
                                                                                                         Html = new Content {
                                                                                                                                Charset = "UTF-8",
                                                                                                                                Data = htmlBody
                                                                                                                            },
                                                                                                         Text = new Content {
                                                                                                                                Charset = "UTF-8",
                                                                                                                                Data = textBody
                                                                                                                            }
                                                                                                     }
                                                                                 },

                                                           // If you are not using a configuration set, comment
                                                           // or remove the following line 
                                                           ConfigurationSetName = "ConfigTeste1"
                                                       };

                try {
					
                    var response = client.SendEmail(sendRequest);
					
					
					return Json(response, JsonRequestBehavior.AllowGet);
					
                } catch (Exception ex) {

					return Json(new { ex.Message, ex.StackTrace}, JsonRequestBehavior.AllowGet);

                }
            }
            
        }


    }

}
