using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BLL.Arquivos;
using DAL.Arquivos;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.Arquivos.ViewModels;

namespace WEB.Areas.Arquivos.Controllers {
	public class ArquivoController : Controller {

		//Constantes

	    //Atributos
	    private IArquivoUploadBL _ArquivoUploadBL; 
	    private IArquivoUploadFotoBL _IArquivoUploadFotoBL; 

	    //Propriedades
	    private IArquivoUploadBL OArquivoUploadBL => _ArquivoUploadBL = _ArquivoUploadBL ?? new ArquivoUploadBL();
	    private IArquivoUploadFotoBL OArquivoUploadFotoBL => _IArquivoUploadFotoBL = _IArquivoUploadFotoBL ?? new ArquivoUploadFotoBL(); 
		
		//Eventos


		//Constructor

		//Listagem de arquivos em formato imagem 
		[ActionName("listar-galeria-fotos")]
		public PartialViewResult listarGaleriaFotos(int idReferencia, string entidade, bool flagPrimeiraImagem = false) {

            if(idReferencia == 0) return PartialView(new List<ArquivoUpload>());

            var listaArquivos = this.OArquivoUploadFotoBL.listar(idReferencia, entidade, "S").OrderBy(x => x.ordem).ThenByDescending(x => x.id).ToList();

			listaArquivos = flagPrimeiraImagem ? listaArquivos.Take(1).ToList() : listaArquivos;

			return PartialView(listaArquivos);
		}

		// Listagem de arquivos diversos formatos 
		[ActionName("listar-galeria-documentos")]
		public PartialViewResult listarDocumentos(int idReferencia, string entidade) {

            if (idReferencia == 0) return PartialView(new List<ArquivoUpload>());

            List<ArquivoUpload> listaArquivos = this.OArquivoUploadBL.listarDocumentos(idReferencia, entidade).ToList();
			return PartialView(listaArquivos);
		}

        // Listagem de arquivos diversos formatos 
		[ActionName("listar-galeria-audios")]
		public PartialViewResult listarAudios(int idReferencia, string entidade) {

            if (idReferencia == 0) return PartialView(new List<ArquivoUpload>());

            List<ArquivoUpload> listaArquivos = this.OArquivoUploadBL.listarAudios(idReferencia, entidade).ToList();
			return PartialView(listaArquivos);
		}


		//Listagem de arquivos de acordo com parametros
		//public PartialViewResult listar(int idReferenciaEntidade, string entidade, string categoria, bool showFlagPrincipal = false) {
		//	var queryArquivos = this.OArquivoUploadBL.listar(idReferenciaEntidade, entidade, categoria, "");

		//	List<ArquivoUpload> listaArquivos = queryArquivos.OrderByDescending(x => x.id).ToList();
		//	return PartialView(listaArquivos);
		//}

		//Atualizaçao de dados do arquivo através do bootstrap-editable
		[HttpPost, ActionName("alterar-dados")]
		public ActionResult alterarDados(FormCollection Form) {
			int idArquivo = UtilRequest.getInt32("pk");
			string nomeCampoAlterado = UtilRequest.getString("name");
			string novoValorCampo = UtilRequest.getString("value");

			this.OArquivoUploadBL.atualizarDados(idArquivo, nomeCampoAlterado, novoValorCampo);

			return Json(new { error = false, message = "Informações atualizadas com sucesso." });
		}

		//Carregao de edicao de logotipo
		[HttpGet, ActionName("editar-logotipo")]
		public ActionResult editarLogoTipo(int? id, int idReferencia, string entidade) {

			ArquivoUploadVM ViewModel = this.carregarViewModel(id, idReferencia, entidade, ArquivoUploadTypes.LOGOTIPO);

			return View(ViewModel);
		}

		//Edicao de logotipo
		[HttpPost, ActionName("editar-logotipo")]
		public ActionResult editarLogoTipo(ArquivoUploadVM ViewModel) {

			if (!ModelState.IsValid) { 
				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Dados inválidos. Por favor, tente novamente.");
				return View(ViewModel);
			}

			bool flagSucesso = this.OArquivoUploadBL.salvarLogotipo(ViewModel.idReferenciaEntidade, ViewModel.entidade, ViewModel.FileUpload);

			if (flagSucesso) {
			    this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O Arquivo foi salvo com sucesso!");
			}

			return View(ViewModel);
		}

		//Carregamento de fotos
		[HttpGet, ActionName("editar-foto")]
		public ActionResult editarFoto(int? id, int idReferencia, string entidade) {

			ArquivoUploadVM ViewModel = this.carregarViewModel(id, idReferencia, entidade, ArquivoUploadTypes.FOTO);

			return View(ViewModel);
		}

		//Salvar uma foto editada
		[HttpPost, ActionName("editar-foto")]
		public ActionResult editarFoto(ArquivoUploadVM ViewModel) {

			if (!ModelState.IsValid) { 
				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Dados inválidos. Por favor, tente novamente.");
				return View(ViewModel);
			}

		    var OArquivo = new ArquivoUpload();
            
		    OArquivo.idReferenciaEntidade = ViewModel.idReferenciaEntidade;

			OArquivo.entidade = ViewModel.entidade;

			OArquivo.idUsuarioCadastro = User.id();

		    bool flagSucesso = this.OArquivoUploadFotoBL.salvar(OArquivo, ViewModel.FileUpload);

			if (flagSucesso) {
				 this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O Arquivo foi salvo com sucesso!");
			}

			if (!String.IsNullOrEmpty(ViewModel.urlRedirect)) { 
				return Redirect(ViewModel.urlRedirect);
			}

			return View(ViewModel);
		}

		//Carregamento de documento para edicao
		[HttpGet, ActionName("editar-documento")]
		public ActionResult editarDocumento(int? id, int idReferencia, string entidade) {

			ArquivoUploadVM ViewModel  = this.carregarViewModel(id, idReferencia, entidade, ArquivoUploadTypes.DOCUMENTO);

			return View(ViewModel);
		}

		//Edicao de carregamento de documento
		[HttpPost, ActionName("editar-documento")]
		public ActionResult editarDocumento(ArquivoUploadVM ViewModel) {

			if (!ModelState.IsValid) { 
				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Dados inválidos. Por favor, tente novamente.");
				return View(ViewModel);
			}

			bool flagSucesso = this.OArquivoUploadBL.salvarDocumento(ViewModel.idReferenciaEntidade, ViewModel.entidade, ViewModel.legenda, ViewModel.FileUpload, User.idOrganizacao(), User.id());

			if (flagSucesso) {
				 this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O Arquivo foi salvo com sucesso!");
			}

			if (!String.IsNullOrEmpty(ViewModel.urlRedirect)) { 
				return Redirect(ViewModel.urlRedirect);
			}

			return Json(new{ error = false});

		}


		//Formulario para arquivos genéricos
		//public PartialViewResult editar(int? id, string entidade, int? idReferenciaEntidade, string categoria) {
			
		//	ArquivoUploadVM ViewModel = this.carregarViewModel(id, idReferenciaEntidade, entidade, categoria);

		//	return PartialView(ViewModel);
		//}

		//
		[HttpPost, ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			ArquivoUpload OArquivoUpload = this.OArquivoUploadBL.alterarStatus(id);
			return Json(new { active = OArquivoUpload.ativo });
		}


		//
		[HttpPost]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach(int idExclusao in id){
				bool flagSucesso = this.OArquivoUploadBL.excluir(idExclusao);

				if (!flagSucesso) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser removidos.";
				}
			}

            Retorno.message = "Registro(s) removido(s) com sucesso.";

			return Json(Retorno);
		}

        // Listagem de arquivos diversos formatos 
        [ActionName("partial-documento")]
        public ActionResult partialDocumento(int? id, int idReferencia, string entidade) {

            ArquivoUploadVM ViewModel = this.carregarViewModel(id, idReferencia, entidade, ArquivoUploadTypes.DOCUMENTO);

            ViewModel.flagView = UtilRequest.getBool("flagView");

            return View(ViewModel);
        }

        //
        public ActionResult ordenar(string[] orderItem) {

            if (orderItem != null && orderItem.Length > 0) {

                var ids = orderItem.Where(s => !String.IsNullOrEmpty(s)).Select(s => Convert.ToInt32(UtilString.onlyNumber(s))).ToArray();

                this.OArquivoUploadBL.salvarOrder(ids);
            }

            return Json(new { error = false });
        }

        #region NONACTIONS

        //Tratamento para pre carregar informações na view model
        private ArquivoUploadVM carregarViewModel(int? id, int? idReferencia, string entidade, string categoria){

			ArquivoUploadVM ViewModel = new ArquivoUploadVM();

			var OArquivo = this.OArquivoUploadBL.carregar(id.toInt()) ?? new ArquivoUpload();

            Mapper.Map(OArquivo, ViewModel);

			if (OArquivo.id == 0) {

				ViewModel.idReferenciaEntidade = UtilNumber.toInt32(idReferencia);

                ViewModel.entidade = entidade;

                ViewModel.categoria = categoria;
			}

			return ViewModel;
		}
			

		#endregion
	}
}
