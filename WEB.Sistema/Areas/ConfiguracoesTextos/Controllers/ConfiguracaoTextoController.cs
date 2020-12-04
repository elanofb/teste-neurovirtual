using System.Web.Mvc;
using BLL.ConfiguracoesTextos;
using WEB.App_Infrastructure;
using WEB.Areas.ConfiguracoesTextos.ViewModels;

namespace WEB.Areas.ConfiguracoesTextos.Controllers {

	[OrganizacaoFilter]
    public class ConfiguracaoTextoController : BaseSistemaController {

		//Atributos
		private IConfiguracaoTextoBL _ConfiguracaoTextoBL;

        //Propriedades
        private IConfiguracaoTextoBL OConfiguracaoTextoBL => this._ConfiguracaoTextoBL = this._ConfiguracaoTextoBL ?? new ConfiguracaoTextoBL();

	    [HttpGet]
	    public ActionResult editar(){

	        var ViewModel = new ConfiguracaoTextoVM();
			
	        ViewModel.carregar();
	        ViewModel.carregarIdiomas();

	        return View(ViewModel);
	    }

	    //
	    [HttpGet, ActionName("modal-editar")]
	    public ActionResult modalEditar(string key) {

	        var ViewModel = new ConfiguracaoTextoForm();
            
	        ViewModel.carregar(key);
            
	        return View(ViewModel);
	    }

	    //
	    [HttpPost, ActionName("salvar-configuracao-texto"), ValidateInput(false)]
	    public ActionResult salvarConfiguracaoTexto(ConfiguracaoTextoForm ViewModel) {

	        if(!ModelState.IsValid){
		        
	            ViewModel.carregar(ViewModel.ConfiguracaoTextoPadrao.key);
		        
	            return View("modal-editar", ViewModel);
	        }
	        
	        foreach (var OConfiguracaoTexto in ViewModel.listaConfiguracaoTexto) {
	            OConfiguracaoTexto.key = ViewModel.ConfiguracaoTextoPadrao.key;
	            OConfiguracaoTextoBL.salvar(OConfiguracaoTexto);
	        }

	        bool flagSucesso = OConfiguracaoTextoBL.salvar(ViewModel.ConfiguracaoTextoPadrao);

	        return Json(new { error = !flagSucesso, message = (flagSucesso ? "" : "Não foi possível salvar o registro." ) });
	    }

        //
//        [HttpGet, ActionName("modal-visualizar")]
//        public ActionResult modalVisualizar(string id) {
//
//            var ViewModel = new ConfiguracaoTextoForm();
//
//            ViewModel.ConfiguracaoTexto = OConfiguracaoTextoBL.carregar(id) ?? new ConfiguracaoTexto();
//            
//            return View(ViewModel);
//        }
    }
}