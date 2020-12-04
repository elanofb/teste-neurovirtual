using System;
using System.Web.Mvc;
using BLL.OrganizacaoConfiguracoes;
using BLL.Organizacoes;
using DAL.Organizacoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.OrganizacaoConfiguracoes.ViewModels;

namespace WEB.Areas.OrganizacaoConfiguracoes.Controllers {

	[OrganizacaoFilter]
    public class ConfiguracaoDadosAssociadoController : Controller {

		//Atributos
		private IOrganizacaoDadosAssociadoBL _IOrganizacaoDadosAssociadoBL;
		private IOrganizacaoBL _IOrganizacaoBL;

        //Propriedades
	    private IOrganizacaoDadosAssociadoBL OrganizacaoDadosAssociadoBL => _IOrganizacaoDadosAssociadoBL = _IOrganizacaoDadosAssociadoBL ?? new OrganizacaoDadosAssociadoBL();
		private IOrganizacaoBL OrganizacaoBL => _IOrganizacaoBL = _IOrganizacaoBL ?? new OrganizacaoBL();
	    
        //
		[HttpGet]
        public ActionResult Index(){

			int idOrganizacao = User.idOrganizacao();
			
			Organizacao OOrganizacao = this.OrganizacaoBL.carregar(idOrganizacao) ?? new Organizacao();
			
			if (OOrganizacao.idOrganizacaoGestora.toInt() == 0){
				object Message = "A organização informada não possui uma entidade vinculada.";
				return View("~/Areas/Erros/Views/erro/sem-registro.cshtml", Message);
				
			}
			
			var ViewModel = new ConfiguracaoDadosAssociadoForm();

			ViewModel.OrganizacaoDadosAssociado = this.OrganizacaoDadosAssociadoBL.carregar(idOrganizacao, false);
            
		    return View(ViewModel);
        }
		
        //
		[HttpPost, ValidateInput(false)]
        public ActionResult salvar(ConfiguracaoDadosAssociadoForm ViewModel){

			if(!ModelState.IsValid){
				return View("Index", ViewModel);
			}
            
			var flagSucesso = this.OrganizacaoDadosAssociadoBL.salvar(ViewModel.OrganizacaoDadosAssociado);

			if (flagSucesso) {
			
				this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "As configurações foram salvas com sucesso.") );
				
				return RedirectToAction("Index");	
			}

			this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve algum problema ao salvar as configurações.") );

			return View("Index", ViewModel);

        }

	}
}