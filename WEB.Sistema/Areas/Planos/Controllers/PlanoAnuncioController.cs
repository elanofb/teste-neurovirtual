using System;
using System.Web.Mvc;
using System.Json;
using MvcFlashMessages;
using WEB.Areas.Planos.ViewModels;

namespace WEB.Areas.Planos.Controllers {

	public class PlanoAnuncioController : Controller {

		//Constantes

		//Atributos
        private IAnuncioBL _AnuncioBL;

		//Propriedades
		private IAnuncioBL OAnuncioBL => _AnuncioBL = _AnuncioBL ?? new AnuncioBL();
		
		//Construtor
		public PlanoAnuncioController() { 
		}

		//GET : Anuncios/Anuncio/listar
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

			string ativo = UtilRequest.getString("flagAtivo");

            int idTipoAnuncio = (int)TipoAnuncioEnum.VITRINE;

			var lista = this.OAnuncioBL.listar(descricao, ativo, idTipoAnuncio).OrderBy(x => x.titulo);

			return View(lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));

		}

		//
		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new PlanoAnuncioForm();

			ViewModel.Anuncio = this.OAnuncioBL.carregarPorContratacao(id.toInt()) ?? new Anuncio();

            ViewModel.Anuncio.idPlanoContratacao = id.toInt();

			return View(ViewModel);
		}

		//
		[HttpPost, ValidateInput(false)]
		public ActionResult editar(PlanoAnuncioForm ViewModel) {

			if (!ModelState.IsValid) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o registro. Tente novamente.");
				return View(ViewModel);
			}

			bool flagSucesso = this.OAnuncioBL.salvar(ViewModel.Anuncio, ViewModel.OArquivo);
            
            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }
            		
            if (flagSucesso) {
                return RedirectToAction("editar", new { id = ViewModel.Anuncio.idPlanoContratacao });
            }

            return View(ViewModel);
		}

		//Post: Anuncios/Anuncio/excluir
		[HttpPost]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				UtilRetorno RetornoExclusao = this.OAnuncioBL.excluir(idExclusao);
				
				if (RetornoExclusao.flagError) { 
					Retorno.error = true;
					Retorno.message = "Algumas exclusões não puderam ser realizadas, tente novamente.";
				}
			}

            return Json(Retorno);
		}
        
	}

}