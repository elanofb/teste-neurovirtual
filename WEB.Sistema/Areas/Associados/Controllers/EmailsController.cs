using System;
using System.Web.Mvc;
using BLL.Emails;
using MvcFlashMessages;
using WEB.Areas.Associados.ViewModels;

namespace WEB.Areas.Associados.Controllers {

	[OrganizacaoFilter]
	public class EmailsController : Controller {

		// Atributos
		private IMensagemEmailCadastroBL _MensagemEmailCadastroBL;
        
		// Propriedades
		private IMensagemEmailCadastroBL OMensagemEmailCadastroBL => _MensagemEmailCadastroBL = _MensagemEmailCadastroBL ?? new MensagemEmailCadastroBL();
        
		[HttpGet]
		public ActionResult index() {
                                        
			var ViewModel = new ConfiguracaoEmailForm();
			
			ViewModel.carregarEmails();
		    		                
			return View(ViewModel);

		}
        
		[ActionName("partial-form")]
		public ActionResult partialForm(ConfiguracaoEmailForm ViewModel) {
			return View(ViewModel);
		}
        
		[ValidateInput(false)]
		[HttpPost, ActionName("salvar")]
		public ActionResult salvar(ConfiguracaoEmailForm ViewModel) {
            
			if (!ModelState.IsValid){
				return PartialView("partial-form", ViewModel);
			}
            
			this.OMensagemEmailCadastroBL.salvar(ViewModel.MensagemEmailAtualizacaoCadastral);
            
			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "As configurações foram salvas com sucesso!");
            
			return PartialView("partial-form", ViewModel);
            
		}

	}
}