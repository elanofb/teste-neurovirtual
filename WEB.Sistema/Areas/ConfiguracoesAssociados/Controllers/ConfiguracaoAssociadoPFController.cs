using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesAssociados.ViewModels;

namespace WEB.Areas.ConfiguracoesAssociados.Controllers {

    public class ConfiguracaoAssociadoPFController : Controller {

		//Atributos
		private IConfiguracaoAssociadoPFBL _IConfiguracaoAssociadoPFBL;

        //Propriedades
        private  IConfiguracaoAssociadoPFBL OConfiguracaoAssociadoPFBL => _IConfiguracaoAssociadoPFBL = _IConfiguracaoAssociadoPFBL ?? new ConfiguracaoAssociadoPFBL();


        //
		[HttpGet]
        public ActionResult listar() {

		    int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
		  
		    if (User.idOrganizacao() > 0) {
		        return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
		    }

            var lista = this.OConfiguracaoAssociadoPFBL.listar(idOrganizacao).ToList();
			
            return View(lista);

        }

        //
		[HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

		    if (User.idOrganizacao() > 0) {
		        idOrganizacao = User.idOrganizacao();
		    }

		    ConfiguracaoAssociadoPFForm ViewModel = new ConfiguracaoAssociadoPFForm {
		        ConfiguracaoAssociadoPF = this.OConfiguracaoAssociadoPFBL.carregar(idOrganizacao, false) ?? new ConfiguracaoAssociadoPF(),
            };

		    return View(ViewModel);
        }

        //
		[HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoAssociadoPFForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

		    if (User.idOrganizacao() > 0) {
		        ViewModel.ConfiguracaoAssociadoPF.idOrganizacao = User.idOrganizacao();
		    }

            if (!(ViewModel.ConfiguracaoAssociadoPF.flagRotinaInadimplencia ?? false)) {
                ViewModel.ConfiguracaoAssociadoPF.flagTodosPagamentosAdimplencia = null;
                ViewModel.ConfiguracaoAssociadoPF.qtdeUltimosPagamentosAdimplencia = null;
            }

            if (ViewModel.ConfiguracaoAssociadoPF.flagTodosPagamentosAdimplencia ?? true) {
                ViewModel.ConfiguracaoAssociadoPF.qtdeUltimosPagamentosAdimplencia = null;
            }
            
			this.OConfiguracaoAssociadoPFBL.salvar(ViewModel.ConfiguracaoAssociadoPF);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso.") );

			return RedirectToAction("editar", new { ViewModel.ConfiguracaoAssociadoPF.idOrganizacao });

        }

	}
}