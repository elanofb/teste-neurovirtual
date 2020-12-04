using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesScripts;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesScripts.ViewModels;

namespace WEB.Areas.ConfiguracoesScripts.Controllers {

    public class ConfiguracaoScriptsController : Controller {

		//Atributos
		private IConfiguracaoScriptsBL _IConfiguracaoScriptsBL;

        //Propriedades
        private  IConfiguracaoScriptsBL OConfiguracaoScriptsBL => _IConfiguracaoScriptsBL = _IConfiguracaoScriptsBL ?? new ConfiguracaoScriptsBL();


        //
		[HttpGet]
        public ActionResult listar() {

		    int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
		  
		    if (User.idOrganizacao() > 0) {

		        return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });

		    }

            var lista = this.OConfiguracaoScriptsBL.listar(idOrganizacao).ToList();
			
            return View(lista);

        }

        //
		[HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

		    if (User.idOrganizacao() > 0) {
		        idOrganizacao = User.idOrganizacao();
		    }

		    ConfiguracaoScriptsForm ViewModel = new ConfiguracaoScriptsForm {
		        ConfiguracaoScripts = this.OConfiguracaoScriptsBL.carregar(idOrganizacao, false)
            };

		    return View(ViewModel);
        }

        //
		[HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoScriptsForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

		    if (User.idOrganizacao() > 0) {
		        ViewModel.ConfiguracaoScripts.idOrganizacao = User.idOrganizacao();
		    }
            
			this.OConfiguracaoScriptsBL.salvar(ViewModel.ConfiguracaoScripts);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso.") );

			return RedirectToAction("editar", new { ViewModel.ConfiguracaoScripts.idOrganizacao });

        }

	}
}