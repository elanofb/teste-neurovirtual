using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Configuracao.ViewModels;
using DAL.Configuracoes;
using BLL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;

namespace WEB.Areas.Configuracao.Controllers {

    public class ConfiguracaoOperacaoCompraController : Controller {

		//Atributos
		private ConfiguracaoOperacaoCompraBL _ConfiguracoesSistemaBL;

        //Propriedades
        private  ConfiguracaoOperacaoCompraBL OConfiguracoesSistemaBL => this._ConfiguracoesSistemaBL = this._ConfiguracoesSistemaBL ?? new ConfiguracaoOperacaoCompraBL();


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

		    ConfiguracaoOperacaoCompraForm ViewModel = new ConfiguracaoOperacaoCompraForm{
		        ConfiguracaoOperacaoCompra = this.OConfiguracoesSistemaBL.carregar(idOrganizacao, false) ?? new ConfiguracaoOperacaoCompra(),
            };

		    return View(ViewModel);
        }
		
        //
		[HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoOperacaoCompraForm ViewModel){

			if(!ModelState.IsValid){
				
				return View(ViewModel);
			}

			var percentualTotal = decimal.Add(ViewModel.ConfiguracaoOperacaoCompra.percentualLucro.toDecimal(), ViewModel.ConfiguracaoOperacaoCompra.percentualComissao.toDecimal());
			percentualTotal = decimal.Add(percentualTotal, ViewModel.ConfiguracaoOperacaoCompra.percentualCashback.toDecimal());
			percentualTotal = decimal.Add(percentualTotal, ViewModel.ConfiguracaoOperacaoCompra.percentualIndicacaoNivel1.toDecimal());
			percentualTotal = decimal.Add(percentualTotal, ViewModel.ConfiguracaoOperacaoCompra.percentualIndicacaoNivel2.toDecimal());
			percentualTotal = decimal.Add(percentualTotal, ViewModel.ConfiguracaoOperacaoCompra.percentualIndicacaoNivel3.toDecimal());

			if (percentualTotal != 100) {
				
				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha!", "A soma dos percentuais configurados não equivale à 100%.") );
				
				return View(ViewModel);
			}

		    if (User.idOrganizacao() > 0) {
		        ViewModel.ConfiguracaoOperacaoCompra.idOrganizacao = User.idOrganizacao();
		    }
            
			this.OConfiguracoesSistemaBL.salvar(ViewModel.ConfiguracaoOperacaoCompra);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso.") );

			return RedirectToAction("editar", new {ViewModel.ConfiguracaoOperacaoCompra.Organizacao});
        }
	}
}