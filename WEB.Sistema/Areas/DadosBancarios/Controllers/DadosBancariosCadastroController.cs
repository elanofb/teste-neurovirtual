using System;
using System.Web.Mvc;
using BLL.DadosBancarios.Interfaces;
using BLL.DadosBancarios.Services;
using WEB.App_Infrastructure;
using WEB.Areas.DadosBancarios.ViewModels;

namespace WEB.Areas.DadosBancarios.Controllers {
    
    public class DadosBancariosCadastroController : BaseSistemaController {
        
	    // Atributos
	    private IDadoBancarioCadastroBL _IDadoBancarioCadastroBL;
        
	    // Serviços
	    private IDadoBancarioCadastroBL ODadoBancarioCadastroBL => _IDadoBancarioCadastroBL = _IDadoBancarioCadastroBL ?? new DadoBancarioCadastroBL();
	    
        //
        [HttpGet, ActionName("modal-cadastro")]
        public ActionResult modalCadastro(int? idPessoa) {
			
            var ViewModel = new DadosBancariosForm();
            
            ViewModel.DadoBancario.idPessoa = idPessoa.toInt();
			
            return PartialView(ViewModel);
			
        }
        
        //
        [HttpGet, ActionName("modal-editar")]
        public ActionResult modalEditar(int? id) {
			
            var ViewModel = new DadosBancariosForm();
	        
	        ViewModel.carregarInformacoes(id.toInt());
			
            return PartialView("modal-cadastro", ViewModel);
			
        }
        
        [HttpPost, ActionName("salvar")]
        public ActionResult salvar(DadosBancariosForm ViewModel) {
            
            if (!ModelState.IsValid) {
                return PartialView("modal-cadastro", ViewModel);
            }

            bool flagSucesso = ViewModel.salvar();

	        ViewModel.carregarInformacoes(ViewModel.DadoBancario.id);
	        
	        var id = ViewModel.DadoBancario.id;
	        var text = String.Concat(ViewModel.DadoBancario.Banco.descricao, " - ", ViewModel.DadoBancario.nroConta);
            
            return Json(new { error = !flagSucesso, id, text });
			
        }
	    
	    [HttpPost, ActionName("alterar-status")]
	    public ActionResult alterarStatus(int id) {
		    return Json(this.ODadoBancarioCadastroBL.alterarStatus(id));
	    }
        
    }
}