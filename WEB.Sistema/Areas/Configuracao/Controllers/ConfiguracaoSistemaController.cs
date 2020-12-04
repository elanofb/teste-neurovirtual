using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Configuracao.ViewModels;
using DAL.Configuracoes;
using BLL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;

namespace WEB.Areas.Configuracao.Controllers {

    public class ConfiguracaoSistemaController : Controller {

		//Atributos
		private ConfiguracaoSistemaBL _ConfiguracoesSistemaBL;

        //Propriedades
        private  ConfiguracaoSistemaBL OConfiguracoesSistemaBL => this._ConfiguracoesSistemaBL = this._ConfiguracoesSistemaBL ?? new ConfiguracaoSistemaBL();


        //
		[HttpGet]
        public ActionResult listar() {

		    int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
		  
		    if (User.idOrganizacao() > 0) {
		        return RedirectToAction("editar", new {idOrganizacao = User.idOrganizacao()});
		    }

            var lista = this.OConfiguracoesSistemaBL.listar(idOrganizacao).ToList();
			
            return View(lista);
        }

        //
		[HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

		    if (User.idOrganizacao() > 0) {
		        idOrganizacao = User.idOrganizacao();
		    }

		    ConfiguracaoSistemaForm ViewModel = new ConfiguracaoSistemaForm{
		        ConfiguracaoSistema = this.OConfiguracoesSistemaBL.carregar(idOrganizacao, false) ?? new ConfiguracaoSistema(),
            };

		    return View(ViewModel);
        }

        //
		[HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoSistemaForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

		    if (User.idOrganizacao() > 0) {
		        ViewModel.ConfiguracaoSistema.idOrganizacao = User.idOrganizacao();
		    }
            
			this.OConfiguracoesSistemaBL.salvar(ViewModel.ConfiguracaoSistema);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso.") );

			return RedirectToAction("editar", new {ViewModel.ConfiguracaoSistema.Organizacao});
        }
	}
}