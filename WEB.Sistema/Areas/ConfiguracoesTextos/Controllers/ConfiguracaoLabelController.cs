using System.Web.Mvc;
using BLL.ConfiguracoesTextos;
using WEB.App_Infrastructure;
using WEB.Areas.ConfiguracoesTextos.ViewModels;

namespace WEB.Areas.ConfiguracoesTextos.Controllers {

	[OrganizacaoFilter]
    public class ConfiguracaoLabelController : BaseSistemaController {

		//Atributos
		private IConfiguracaoLabelBL _ConfiguracaoLabelBL;

        //Propriedades
        private IConfiguracaoLabelBL OConfiguracaoLabelBL => this._ConfiguracaoLabelBL = this._ConfiguracaoLabelBL ?? new ConfiguracaoLabelBL();

        //
		[HttpGet]
        public ActionResult editar(){

            var ViewModel = new ConfiguracaoLabelVM();
			
		    ViewModel.carregar();
		    ViewModel.carregarIdiomas();

            return View(ViewModel);
        }

        //
        [HttpGet, ActionName("modal-editar")]
        public ActionResult modalEditar(string key) {

            var ViewModel = new ConfiguracaoLabelForm();
            
            ViewModel.carregar(key);
            
            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("salvar-configuracao-label"), ValidateInput(false)]
        public ActionResult salvarConfiguracaoLabel(ConfiguracaoLabelForm ViewModel) {

	        if(!ModelState.IsValid){
		        
		        ViewModel.carregar(ViewModel.ConfiguracaoLabelPadrao.key);
		        
		        return View("modal-editar", ViewModel);
	        }
	        
            foreach (var OConfiguracaoLabel in ViewModel.listaConfiguracaoLabel) {
                OConfiguracaoLabel.key = ViewModel.ConfiguracaoLabelPadrao.key;
                OConfiguracaoLabelBL.salvar(OConfiguracaoLabel);
            }

            bool flagSucesso = OConfiguracaoLabelBL.salvar(ViewModel.ConfiguracaoLabelPadrao);

            return Json(new { error = !flagSucesso, message = (flagSucesso ? "" : "Não foi possível salvar o registro." ) });
        }

        //
//        [HttpGet, ActionName("modal-visualizar")]
//        public ActionResult modalVisualizar(string key) {
//
//            var ViewModel = new ConfiguracaoLabelForm();
//
//            ViewModel.ConfiguracaoLabel = OConfiguracaoLabelBL.carregar(key) ?? new ConfiguracaoLabel();
//            
//            return View(ViewModel);
//        }
    }
}