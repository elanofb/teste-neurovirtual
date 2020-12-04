using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Configuracao.ViewModels;
using DAL.Configuracoes;
using BLL.Configuracoes;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;

namespace WEB.Areas.Configuracao.Controllers {

	[OrganizacaoFilter]
    public class ConfiguracaoSaqueController : Controller {
		
		//Atributos
		private ConfiguracaoSaqueBL _ConfiguracoesSistemaBL;
		
        //Propriedades
        private  ConfiguracaoSaqueBL OConfiguracoesSistemaBL => this._ConfiguracoesSistemaBL = this._ConfiguracoesSistemaBL ?? new ConfiguracaoSaqueBL();

        //
		[HttpGet]
        public ActionResult listar() {
			
		    int idOrganizacao = User.idOrganizacao();		  	
			
            var query = this.OConfiguracoesSistemaBL.listar(idOrganizacao).Select(x => new{
	            x.id,
	            x.idOrganizacao,
	            x.idTipoCadastro,
	            x.dtCadastro,
	            UsuarioSistema = new{
		            x.UsuarioSistema.nome
	            }
            });
			
			var lista = query.ToListJsonObject<ConfiguracaoSaque>();
				
            return View(lista);
			
        }

        //
		[HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
			
			int idTipoCadastro = UtilRequest.getInt32("idTipoCadastro");
			
		    if (User.idOrganizacao() > 0) {
		        idOrganizacao = User.idOrganizacao();
		    }
			
		    ConfiguracaoSaqueForm ViewModel = new ConfiguracaoSaqueForm{
		        ConfiguracaoSaque = this.OConfiguracoesSistemaBL.carregar(idOrganizacao, idTipoCadastro) ?? new ConfiguracaoSaque(),
            };

		    return View(ViewModel);
			
        }
		
        //
		[HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoSaqueForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

		    if (User.idOrganizacao() > 0) {
		        ViewModel.ConfiguracaoSaque.idOrganizacao = User.idOrganizacao();
		    }
            
			this.OConfiguracoesSistemaBL.salvar(ViewModel.ConfiguracaoSaque);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso.") );

			return RedirectToAction("editar", new { ViewModel.ConfiguracaoSaque.idOrganizacao, ViewModel.ConfiguracaoSaque.idTipoCadastro });
        }
	}
}