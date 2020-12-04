using System;
using System.Web.Mvc;
using BLL.ConfiguracoesTextos;
using WEB.App_Infrastructure;
using WEB.Areas.ConfiguracoesTextos.ViewModels;

namespace WEB.Areas.ConfiguracoesTextos.Controllers {

	[OrganizacaoFilter]
    public class IdiomaCadastroController : BaseSistemaController {

		//Atributos
		private IIdiomaCadastroBL _IdiomaCadastroBL;

        //Propriedades
        private IIdiomaCadastroBL OIdiomaCadastroBL => this._IdiomaCadastroBL = this._IdiomaCadastroBL ?? new IdiomaCadastroBL();

        //
        [HttpGet, ActionName("modal-editar")]
        public ActionResult modalEditar(int? id) {

            var ViewModel = new IdiomaCadastroForm();

            if (id > 0) {
                ViewModel.carregar(id.toInt());
            }
            
            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("salvar-idioma"), ValidateInput(false)]
        public ActionResult salvarIdiomaCadastro(IdiomaCadastroForm ViewModel) {

            if (!ModelState.IsValid){
                return View("modal-editar", ViewModel);
            }

            bool flagSucesso = OIdiomaCadastroBL.salvar(ViewModel.Idioma);

            return Json(new { error = !flagSucesso, message = (flagSucesso ? "" : "Não foi possível salvar o registro." ) });
        }
		
	    //
	    [HttpPost, ActionName("alterar-status")]
	    public ActionResult alterarStatus(int id) {
	        return Json(this.OIdiomaCadastroBL.alterarStatus(id));
	    }
	    
    }
}