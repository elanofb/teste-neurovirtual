using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using WEB.Areas.Publicacoes.ViewModels;
using DAL.Publicacoes;
using PagedList;
using System.Json;
using MvcFlashMessages;

namespace WEB.Areas.Publicacoes.Controllers {

	public class ParceiroController : Controller {

		//Constantes

		//Atributos
        private IParceiroBL _ParceiroBL;

		//Propriedades
		private IParceiroBL OParceiroBL => _ParceiroBL = _ParceiroBL ?? new ParceiroBL();
		
		//Construtor
		public ParceiroController() { 
		}

		//GET : Parceiros/Parceiro/listar
		public ActionResult listar() {
			string descricao = UtilRequest.getString("valorBusca");
			int? idPortal = UtilRequest.getInt32("idPortal");
            string ativo = UtilRequest.getString("flagAtivo");
            int idTipoParceiro = UtilRequest.getInt32("idTipoParceiro");

            var lista = this.OParceiroBL.listar(descricao, ativo, idTipoParceiro, idPortal).OrderBy(x => x.nome);

			return View(lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		//
		[HttpGet]
		public ActionResult editar(int? id) {
			ParceiroForm ViewModel = new ParceiroForm();

			ViewModel.Parceiro = this.OParceiroBL.carregar(UtilNumber.toInt32(id)) ?? new Parceiro();

			return View(ViewModel);
		}

		//
		[HttpPost, ValidateInput(false)]
		public ActionResult editar(ParceiroForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}
            
			bool flagSucesso = this.OParceiroBL.salvar(ViewModel.Parceiro, ViewModel.OArquivo);
            
            if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));
                return RedirectToAction("editar", new { id = ViewModel.Parceiro.id });
            }
		
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
            return View(ViewModel);
		}

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OParceiroBL.alterarStatus(id));
        }

		//Post: Parceiros/Parceiro/excluir
		[HttpPost]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();

			foreach (int idExclusao in id) { 
				UtilRetorno RetornoExclusao = this.OParceiroBL.excluir(idExclusao);
				
				if (RetornoExclusao.flagError) { 
					Retorno.error = true;
					Retorno.message = "Algumas exclusões não puderam ser realizadas, tente novamente.";
				}
			}

		    if (Retorno.error == false) {
		        Retorno.error = false;
		        Retorno.message = "Os registros foram excluídos com sucesso.";
		    }

            return Json(Retorno);
		}   
	}

}