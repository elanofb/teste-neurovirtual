using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesRecibo;
using DAL.ConfiguracoesRecibo;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesRecibo.ViewModels;

namespace WEB.Areas.ConfiguracoesRecibo.Controllers {

    public class ConfiguracaoReciboController : Controller {

		//Atributos
		private IConfiguracaoReciboBL _IConfiguracaoReciboBL;

        //Propriedades
        private  IConfiguracaoReciboBL OConfiguracaoReciboBL => _IConfiguracaoReciboBL = _IConfiguracaoReciboBL ?? new ConfiguracaoReciboBL();


        //
		[HttpGet]
        public ActionResult listar() {

		    int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
		  
		    if (User.idOrganizacao() > 0) {
		        return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
		    }

            var lista = this.OConfiguracaoReciboBL.listar(idOrganizacao).ToList();
			
            return View(lista);

        }

        //
		[HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

		    if (User.idOrganizacao() > 0) {
		        idOrganizacao = User.idOrganizacao();
		    }

		    ConfiguracaoReciboForm ViewModel = new ConfiguracaoReciboForm {
		        ConfiguracaoRecibo = this.OConfiguracaoReciboBL.carregar(idOrganizacao, false) ?? new ConfiguracaoRecibo(),
            };

		    return View(ViewModel);
        }

        //
		[HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoReciboForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

		    if (User.idOrganizacao() > 0) {
		        ViewModel.ConfiguracaoRecibo.idOrganizacao = User.idOrganizacao();
		    }
            
			this.OConfiguracaoReciboBL.salvar(ViewModel.ConfiguracaoRecibo);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso.") );

			return RedirectToAction("editar", new { ViewModel.ConfiguracaoRecibo.idOrganizacao });

        }

	}
}