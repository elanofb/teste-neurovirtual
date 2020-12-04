using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesCarteirinha;
using DAL.ConfiguracoesCateirinha;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesCarteirinha.ViewModels;

namespace WEB.Areas.ConfiguracoesCarteirinha.Controllers {

    public class ConfiguracaoCarteirinhaController : Controller {

		//Atributos
		private IConfiguracaoCarteirinhaBL _IConfiguracaoCarteirinhaBL;

        //Propriedades
        private  IConfiguracaoCarteirinhaBL OConfiguracaoCarteirinhaBL => _IConfiguracaoCarteirinhaBL = _IConfiguracaoCarteirinhaBL ?? new ConfiguracaoCarteirinhaBL();


        //
		[HttpGet]
        public ActionResult listar() {

		    int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
		  
		    if (User.idOrganizacao() > 0) {
		        return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
		    }

            var lista = this.OConfiguracaoCarteirinhaBL.listar(idOrganizacao).ToList();
			
            return View(lista);

        }

        //
		[HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

		    if (User.idOrganizacao() > 0) {
		        idOrganizacao = User.idOrganizacao();
		    }

		    ConfiguracaoCarteirinhaForm ViewModel = new ConfiguracaoCarteirinhaForm {
		        ConfiguracaoCarteirinha = this.OConfiguracaoCarteirinhaBL.carregar(idOrganizacao) ?? new ConfiguracaoCarteirinha(),
            };

		    return View(ViewModel);
        }

        //
		[HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoCarteirinhaForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

		    if (User.idOrganizacao() > 0) {
		        ViewModel.ConfiguracaoCarteirinha.idOrganizacao = User.idOrganizacao();
		    }
            
			this.OConfiguracaoCarteirinhaBL.salvar(ViewModel.ConfiguracaoCarteirinha);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso.") );

			return RedirectToAction("editar", new { ViewModel.ConfiguracaoCarteirinha.idOrganizacao });

        }

	}
}