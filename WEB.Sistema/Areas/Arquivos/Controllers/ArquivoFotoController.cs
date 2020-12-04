using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Arquivos;
using BLL.Services;
using DAL.Arquivos;
using MvcFlashMessages;
using WEB.Areas.Arquivos.ViewModels;

namespace WEB.Areas.Arquivos.Controllers {

	public class ArquivoFotoController : Controller {
        
		//Atributos
		private IArquivoUploadFotoBL _IArquivoUploadFotoBL; 

		//Propriedades
		protected IArquivoUploadFotoBL OArquivoUploadFotoBL => _IArquivoUploadFotoBL = _IArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();
		
		//Listagem de arquivos em formato imagem 
		[ActionName("partial-lista-fotos")]
		public PartialViewResult listarFotos(int idReferencia, string entidade, string tipoExibicao = "tabela") {

            var viewName = "partial-lista-fotos-" + tipoExibicao;

            if(idReferencia == 0) {
	            
                return PartialView(viewName, new List<ArquivoUpload>());
	            
            }

            var listaArquivos = this.OArquivoUploadFotoBL.listar(idReferencia, entidade, "")
	            										.Select( x => new {
															x.id, 
															x.idOrganizacao,
															x.ordem,
															x.categoria,
															x.entidade,
															x.contentType,
															x.legenda,
															x.extensao,
															x.nomeArquivo,
															x.idReferenciaEntidade,
															x.titulo,
															x.path,
															x.pathThumb,
															x.dtCadastro,
		            										x.ativo
														}).ToListJsonObject<ArquivoUpload>()
														.OrderBy(x => x.ordem)
														.ThenByDescending(x => x.id)
														.ToList();

			return PartialView(viewName, listaArquivos);
		}
        
		//Carregamento de fotos
		[HttpGet, ActionName("partial-editar")]
		public ActionResult partialEditar(int? id, int idReferencia, string entidade, string returnUrl, string tipoExibicao = "horizontal") {

			var ViewModel = this.carregarViewModel(id, idReferencia, entidade);

		    ViewModel.tipoExibicao = tipoExibicao;

		    var viewName = "partial-editar-" + ViewModel.tipoExibicao;

		    ViewModel.urlRedirect = returnUrl;

			return View(viewName, ViewModel);
		}
        
		//Salvar uma foto editada
		[HttpPost]
		public ActionResult salvar(ArquivoUploadFotoVM ViewModel) {

		    var viewName = "partial-editar-" + ViewModel.tipoExibicao;

			if (!ModelState.IsValid) { 
				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Dados inválidos. Por favor, tente novamente.");
				return View(viewName, ViewModel);
			}

		    foreach (var FileUploadItem in ViewModel.FileUpload) {
		    
                var OArquivo = ViewModel.OArquivo.ToJsonObject<ArquivoUpload>();

                OArquivo.id = 0;

		        this.OArquivoUploadFotoBL.salvar(OArquivo, FileUploadItem);
                    
		    }
            
            return Json(new { error = false, message = "O(s) arquivo(s) foi salvo com sucesso!" }, JsonRequestBehavior.AllowGet);
			
		}
        
	    //Atualizaçao de dados do arquivo através do bootstrap-editable
	    [HttpPost, ActionName("alterar-dados")]
	    public ActionResult alterarDados(FormCollection Form) {

	        int idArquivo = UtilRequest.getInt32("pk");

	        string nomeCampoAlterado = UtilRequest.getString("name");

	        string novoValorCampo = UtilRequest.getString("value");

	        this.OArquivoUploadFotoBL.atualizarDados(idArquivo, nomeCampoAlterado, novoValorCampo);

	        return Json(new { error = false, message = "Informações atualizadas com sucesso." });
	    }

	    //
	    [HttpPost, ActionName("registrar-foto-principal")]
	    public ActionResult registrarFotoPrincipal(int id) {
	        return Json(this.OArquivoUploadFotoBL.registrarFotoPrincipal(id), JsonRequestBehavior.AllowGet);
	    }

	    //
	    [ActionName("alterar-ordem")]
	    public ActionResult alterarOrdem(int id, byte pos) {

	        this.OArquivoUploadFotoBL.reordenarExibicao(id, pos);

	        return null;
	    }

		//
		[HttpPost, ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.OArquivoUploadFotoBL.alterarStatus(id), JsonRequestBehavior.AllowGet);
		}
        
		//
		[HttpPost]
		public ActionResult excluir(int id) {
			return Json(this.OArquivoUploadFotoBL.excluir(id), JsonRequestBehavior.AllowGet);
		}
        
        #region NONACTIONS

        //Tratamento para pre carregar informações na view model
        private ArquivoUploadFotoVM carregarViewModel(int? id, int? idReferencia, string entidade){

			var ViewModel = new ArquivoUploadFotoVM();

            ViewModel.OArquivo = this.OArquivoUploadFotoBL.carregar(id.toInt());

			if (ViewModel.OArquivo == null) {

			    ViewModel.OArquivo = new ArquivoUpload();

				ViewModel.OArquivo.idReferenciaEntidade = idReferencia.toInt();

				ViewModel.OArquivo.entidade = entidade;

				ViewModel.OArquivo.categoria = ArquivoUploadTypes.FOTO;

			}

			return ViewModel;
		}
			

		#endregion

	}

}
