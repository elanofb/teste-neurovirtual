using System;
using System.Web.Mvc;
using BLL.Pessoas;
using DAL.Pessoas;
using WEB.App_Infrastructure;
using WEB.Areas.Pessoas.ViewModels;
using System.Json;
using BLL.Arquivos;
using DAL.Entities;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;

namespace WEB.Areas.Pessoas.Controllers{

    public class PessoaRelacionamentoCadastroController : BaseSistemaController{

		//Atributos
	    private IPessoaRelacionamentoBL _PessoaRelacionamentoBL; 
	    private IArquivoUploadBL _IArquivoUploadBL; 

		//Propriedades
	    private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL());
	    private IArquivoUploadBL OArquivoUploadBL => (this._IArquivoUploadBL = this._IArquivoUploadBL ?? new ArquivoUploadBL());

		//Formulário Parcial para nova ocorrência de relacionamento
		[ActionName("modal-form-cadastro"), HttpGet]
		public PartialViewResult modalFormCadastro(int? id, int? idPessoa){

			var ViewModel = new PessoaRelacionamentoForm();
			 
			ViewModel.PessoaRelacionamento = this.OPessoaRelacionamentoBL.carregar(UtilNumber.toInt32(id)) ?? new PessoaRelacionamento() { idPessoa = idPessoa.toInt() };

			ViewModel.flagRecarregar = UtilRequest.getBool("flagRecarregar") ?? false;
			
			return PartialView(ViewModel);
		}


		//Formulario submetido para nova ocorrência
		[HttpPost]
		public ActionResult salvar(PessoaRelacionamentoForm ViewModel){

			if (!ModelState.IsValid) {
				return PartialView("modal-form-cadastro", ViewModel);
			}

			bool flagSucesso = this.OPessoaRelacionamentoBL.salvar(ViewModel.PessoaRelacionamento);

			if (flagSucesso) {

				foreach (var OArquivo in ViewModel.Arquivos)
				{
					if (OArquivo.FileUpload != null){
						OArquivoUploadBL.salvarDocumento(ViewModel.PessoaRelacionamento.id, EntityTypes.PESSOADOCUMENTO_RELACIONAMENTO, OArquivo.legenda, OArquivo.FileUpload, User.idOrganizacao(), User.id());	
					}									
				}	

				this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

				return Json(new { error = false, ViewModel.flagRecarregar }, JsonRequestBehavior.AllowGet);
			}

			this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro", "Houve algum problema ao salvar os dados da ocorrência."));

			return PartialView("modal-form-cadastro", ViewModel);
			
		}

        //Excluir determinado registro
        [HttpPost]
        public ActionResult excluir(int[] id) {
	        
			JsonMessage Retorno = new JsonMessage();
	        
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				
				UtilRetorno RetornoExclusao = this.OPessoaRelacionamentoBL.excluir(idExclusao);
				
				if (RetornoExclusao.flagError) { 
					Retorno.error = false;
				}
				
			}

            Retorno.message = "O(s) registro(s) foi(ram) removido(s) com sucesso.";

            return Json(Retorno);
        }
    }
}
