using System;
using System.Web.Mvc;
using BLL.Arquivos;
using BLL.Atendimentos;
using DAL.Arquivos;
using DAL.Entities;
using WEB.Areas.Atendimentos.ViewModels;

namespace WEB.Areas.Atendimentos.Controllers {

    [OrganizacaoFilter]
	public class AtendimentoCadastroController : Controller {

		//Atributos
		private IAtendimentoCadastroBL _AtendimentoCadastroBL;
		private IArquivoUploadBL _ArquivoUploadBL;

		//Propriedades
		private IAtendimentoCadastroBL OAtendimentoCadastroBL => _AtendimentoCadastroBL = _AtendimentoCadastroBL ?? new AtendimentoCadastroBL();
		private IArquivoUploadBL OArquivoUploadBL => _ArquivoUploadBL = _ArquivoUploadBL ?? new ArquivoUploadBL();

        //
	    [HttpGet]
		public ActionResult index() {

			var ViewModel = new AtendimentoForm();
            
            return View(ViewModel);

		}
	    
        //
	    [HttpPost]
		public ActionResult salvar(AtendimentoForm ViewModel) {

//		    if (!ModelState.IsValid) {
//			    return View("index", ViewModel);
//		    }

		    var flagTipoPessoa = UtilRequest.getString("flagTipoPessoa");

		    if (flagTipoPessoa == "F") {
			    ViewModel.Atendimento.nroDocumento = UtilRequest.getString("cpf");
		    }
		    
		    if (flagTipoPessoa == "J") {
			    ViewModel.Atendimento.nroDocumento = UtilRequest.getString("cnpj");
		    }

		    var flagSucesso = OAtendimentoCadastroBL.salvar(ViewModel.Atendimento);

		    if (flagSucesso) {
			    
			    ArquivoUpload OArquivoUpload = new ArquivoUpload();
			    
			    OArquivoUpload.idReferenciaEntidade = ViewModel.Atendimento.id;
			    OArquivoUpload.legenda = "Arquivo do Atendimento #" + ViewModel.Atendimento.id + " - " + ViewModel.Atendimento.titulo;
			    OArquivoUpload.categoria = ArquivoUploadTypes.DOCUMENTO;
			    OArquivoUpload.entidade = EntityTypes.ATENDIMENTO;

			    foreach (var OArquivo in ViewModel.listaArquivo) {
				    
				    OArquivoUploadBL.salvar(OArquivoUpload, OArquivo);
				    
			    }  

			    return RedirectToAction("detalhe", "Atendimento", new { id = ViewModel.Atendimento.id });
		    }
            
            return View("index", ViewModel);

		}
        
    }
}