using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesAssociados.ViewModels;

namespace WEB.Areas.ConfiguracoesAssociados.Controllers {

    public class ConfiguracaoAssociadoPJController : Controller {

		//Atributos
		private IConfiguracaoAssociadoPJBL _IConfiguracaoAssociadoPJBL;

        //Propriedades
        private  IConfiguracaoAssociadoPJBL OConfiguracaoAssociadoPJBL => _IConfiguracaoAssociadoPJBL = _IConfiguracaoAssociadoPJBL ?? new ConfiguracaoAssociadoPJBL();


        //
		[HttpGet]
        public ActionResult listar() {

		    int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
		  
		    if (User.idOrganizacao() > 0) {
		        return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
		    }

            var lista = this.OConfiguracaoAssociadoPJBL.listar(idOrganizacao).ToList();
			
            return View(lista);

        }

        //
		[HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

		    if (User.idOrganizacao() > 0) {
		        idOrganizacao = User.idOrganizacao();
		    }

		    ConfiguracaoAssociadoPJForm ViewModel = new ConfiguracaoAssociadoPJForm {
		        ConfiguracaoAssociadoPJ = this.OConfiguracaoAssociadoPJBL.carregar(idOrganizacao, false) ?? new ConfiguracaoAssociadoPJ(),
            };

		    return View(ViewModel);
        }

        //
		[HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoAssociadoPJForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

		    if (User.idOrganizacao() > 0) {
		        ViewModel.ConfiguracaoAssociadoPJ.idOrganizacao = User.idOrganizacao();
		    }

            if (!(ViewModel.ConfiguracaoAssociadoPJ.flagRotinaInadimplencia ?? false)) {
                ViewModel.ConfiguracaoAssociadoPJ.flagTodosPagamentosAdimplencia = null;
                ViewModel.ConfiguracaoAssociadoPJ.qtdeUltimosPagamentosAdimplencia = null;
            }

            if (ViewModel.ConfiguracaoAssociadoPJ.flagTodosPagamentosAdimplencia ?? true) {
                ViewModel.ConfiguracaoAssociadoPJ.qtdeUltimosPagamentosAdimplencia = null;
            }

            this.OConfiguracaoAssociadoPJBL.salvar(ViewModel.ConfiguracaoAssociadoPJ);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso.") );

			return RedirectToAction("editar", new { ViewModel.ConfiguracaoAssociadoPJ.idOrganizacao });

        }

	}
}