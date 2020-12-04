using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesEcommerce;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesEcommerce.ViewModels;

namespace WEB.Areas.ConfiguracoesEcommerce.Controllers {

    public class ConfiguracaoEcommerceController : Controller {

		//Atributos
		private IConfiguracaoEcommerceBL _IConfiguracaoEcommerceBL;

        //Propriedades
        private  IConfiguracaoEcommerceBL OConfiguracaoEcommerceBL => _IConfiguracaoEcommerceBL = _IConfiguracaoEcommerceBL ?? new ConfiguracaoEcommerceBL();


        //
		[HttpGet]
        public ActionResult listar() {

		    int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
		  
		    if (User.idOrganizacao() > 0) {
		        return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
		    }

            var lista = this.OConfiguracaoEcommerceBL.listar(idOrganizacao).ToList();
			
            return View(lista);

        }

        //
		[HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

		    if (User.idOrganizacao() > 0) {
		        idOrganizacao = User.idOrganizacao();
		    }

		    var ViewModel = new ConfiguracaoEcommerceForm {
		        ConfiguracaoEcommerce = this.OConfiguracaoEcommerceBL.carregar(idOrganizacao, false)
            };

		    return View(ViewModel);
        }

        //
		[HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoEcommerceForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

		    if (User.idOrganizacao() > 0) {
		        ViewModel.ConfiguracaoEcommerce.idOrganizacao = User.idOrganizacao();
		    }
            
			this.OConfiguracaoEcommerceBL.salvar(ViewModel.ConfiguracaoEcommerce);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso.") );

			return RedirectToAction("editar", new { ViewModel.ConfiguracaoEcommerce.idOrganizacao });

        }

	}
}