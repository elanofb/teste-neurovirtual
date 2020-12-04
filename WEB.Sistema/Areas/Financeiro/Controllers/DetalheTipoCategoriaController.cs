using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;
using DAL.Financeiro;
using PagedList;
using System.Json;
using MvcFlashMessages;

namespace WEB.Areas.Financeiro.Controllers {

	public class DetalheTipoCategoriaController : Controller {

		//Atributos
		private IDetalheTipoCategoriaTituloBL _DetalheTipoCategoriaTituloBL;

		//Propriedades
		private IDetalheTipoCategoriaTituloBL ODetalheTipoCategoriaTituloBL => _DetalheTipoCategoriaTituloBL = _DetalheTipoCategoriaTituloBL ?? new DetalheTipoCategoriaTituloBL();

		public ActionResult listar() {

			var idMacroConta = UtilRequest.getInt32("idMacroConta");
            var idCategoria = UtilRequest.getInt32("idCategoria");
            var idTipoCategoria = UtilRequest.getInt32("idTipoCategoria");
			var descricao = UtilRequest.getString("valorBusca");
			var ativo = UtilRequest.getString("flagAtivo");

			var listaDetalheTipoCategoriaTitulo = this.ODetalheTipoCategoriaTituloBL.listar(idMacroConta, idCategoria, idTipoCategoria, descricao, ativo).OrderBy(x => x.descricao);

			return View(listaDetalheTipoCategoriaTitulo.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new DetalheTipoCategoriaTituloForm();
			var ODetalheTipoCategoriaTitulo = this.ODetalheTipoCategoriaTituloBL.carregar(UtilNumber.toInt32(id)) ?? new DetalheTipoCategoriaTitulo();

			ViewModel.DetalheTipoCategoriaTitulo = ODetalheTipoCategoriaTitulo;
			ViewModel.DetalheTipoCategoriaTitulo.TipoCategoria = ViewModel.DetalheTipoCategoriaTitulo.TipoCategoria ?? new TipoCategoriaTitulo();
			ViewModel.DetalheTipoCategoriaTitulo.TipoCategoria.Categoria = ViewModel.DetalheTipoCategoriaTitulo.TipoCategoria.Categoria ?? new CategoriaTitulo();

			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(DetalheTipoCategoriaTituloForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.ODetalheTipoCategoriaTituloBL.salvar(ViewModel.DetalheTipoCategoriaTitulo);
			
            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.DetalheTipoCategoriaTitulo.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

			return View(ViewModel);
		}

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				bool flagSucesso = this.ODetalheTipoCategoriaTituloBL.excluir(idExclusao);

				if (!flagSucesso) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}

			return Json(Retorno);
		}

        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {
            return Json(this.ODetalheTipoCategoriaTituloBL.alterarStatus(id));
        }

        [ActionName("listar-ajax")]
        public ActionResult listarAjax(int? idTipoCategoria) {

			var query = this.ODetalheTipoCategoriaTituloBL.listar(0, 0, 0, "","S").Where(x => x.idTipoCategoria == idTipoCategoria);
			var lista = query.Select(x => new { value = x.id, text = x.descricao }).Distinct().OrderBy(x => x.text).ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
	}
}