using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Json;
using BLL.Publicacoes;
using DAL.Publicacoes;
using MvcFlashMessages;
using WEB.Areas.Publicacoes.ViewModels;

namespace WEB.Areas.Publicacoes.Controllers {

	public class CategoriaNoticiaController : Controller {

		//Atributos
		private ICategoriaNoticiaBL _CategoriaNoticiaBL;

		//Propriedades
		private ICategoriaNoticiaBL OCategoriaNoticiaBL => _CategoriaNoticiaBL = _CategoriaNoticiaBL ?? new CategoriaNoticiaBL();
		
		public ActionResult listar() {

            var descricao = UtilRequest.getString("valorBusca");
			var ativo = UtilRequest.getBool("flagAtivo");

			var listaCategoriaNoticia = this.OCategoriaNoticiaBL.listar(descricao, ativo).OrderBy(x => x.descricao);

			return View(listaCategoriaNoticia.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new CategoriaNoticiaForm();
			ViewModel.CategoriaNoticia = this.OCategoriaNoticiaBL.carregar(UtilNumber.toInt32(id)) ?? new CategoriaNoticia();

			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(CategoriaNoticiaForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OCategoriaNoticiaBL.salvar(ViewModel.CategoriaNoticia);
			
            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.CategoriaNoticia.id });

            }

			this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
            
			return View(ViewModel);
		}

		[HttpGet, ActionName("modal-editar")]
		public ActionResult modalEditar(int? id) {
			
			var ViewModel = new CategoriaNoticiaForm();

			ViewModel.CategoriaNoticia = this.OCategoriaNoticiaBL.carregar(UtilNumber.toInt32(id)) ?? new CategoriaNoticia();

			return PartialView(ViewModel);
		}

		[HttpPost, ActionName("salvar-modal-editar")]
		public ActionResult salvarModalEditar(CategoriaNoticiaForm ViewModel) {

			if (!ModelState.IsValid) {
				return PartialView("modal-editar", ViewModel);
			}

		    ViewModel.CategoriaNoticia.ativo = true;

			bool flagSucesso = this.OCategoriaNoticiaBL.salvar(ViewModel.CategoriaNoticia);

			return Json(new {error = false, flagSucesso, ViewModel.CategoriaNoticia.id, ViewModel.CategoriaNoticia.descricao});
		}

		[HttpPost]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();

			foreach (int idExclusao in id) { 
				bool flagSucesso = this.OCategoriaNoticiaBL.excluir(idExclusao);

				if (!flagSucesso) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}

		    if (Retorno.error == false) {
		        Retorno.error = false;
		        Retorno.message = "Os registros foram excluídos com sucesso.";
		    }

            return Json(Retorno);
		}

        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {
            return Json(this.OCategoriaNoticiaBL.alterarStatus(id));
        }
	}
}