using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesRedesSociais;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesRedesSociais.ViewModels;

namespace WEB.Areas.ConfiguracoesRedesSociais.Controllers {

    public class ConfiguracaoRedesSociaisController : Controller {

		//Atributos
		private IConfiguracaoRedesSociaisBL _IConfiguracaoRedesSociaisBL;

        //Propriedades
        private  IConfiguracaoRedesSociaisBL OConfiguracaoRedesSociaisBL => _IConfiguracaoRedesSociaisBL = _IConfiguracaoRedesSociaisBL ?? new ConfiguracaoRedesSociaisBL();


        //
		[HttpGet]
        public ActionResult listar() {

		    int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
		  
		    if (User.idOrganizacao() > 0) {
		        return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
		    }

            var lista = this.OConfiguracaoRedesSociaisBL.listar(idOrganizacao).ToList();
			
            return View(lista);

        }

        //
		[HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

		    if (User.idOrganizacao() > 0) {
		        idOrganizacao = User.idOrganizacao();
		    }

		    ConfiguracaoRedesSociaisForm ViewModel = new ConfiguracaoRedesSociaisForm {
		        ConfiguracaoRedesSociais = this.OConfiguracaoRedesSociaisBL.carregar(idOrganizacao, false)
            };

		    return View(ViewModel);
        }

        //
		[HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoRedesSociaisForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

		    if (User.idOrganizacao() > 0) {
		        ViewModel.ConfiguracaoRedesSociais.idOrganizacao = User.idOrganizacao();
		    }
            
			this.OConfiguracaoRedesSociaisBL.salvar(ViewModel.ConfiguracaoRedesSociais);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso.") );

			return RedirectToAction("editar", new { ViewModel.ConfiguracaoRedesSociais.idOrganizacao });

        }

	}
}