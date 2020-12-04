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

	public class TipoCategoriaController : Controller {

		//Atributos
		private ITipoCategoriaTituloBL _TipoCategoriaTituloBL;

		//Propriedades
		private ITipoCategoriaTituloBL OTipoCategoriaTituloBL => _TipoCategoriaTituloBL = _TipoCategoriaTituloBL ?? new TipoCategoriaTituloBL();
		
		public ActionResult listar() {

			var idMacroConta = UtilRequest.getInt32("idMacroConta");
            var idCategoria = UtilRequest.getInt32("idCategoria");
            var descricao = UtilRequest.getString("valorBusca");
			var ativo = UtilRequest.getString("flagAtivo");

			var listaTipoCategoriaTitulo = this.OTipoCategoriaTituloBL.listar(idMacroConta, idCategoria, descricao, ativo).OrderBy(x => x.descricao);

			return View(listaTipoCategoriaTitulo.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new TipoCategoriaTituloForm();
			var OTipoCategoriaTitulo = this.OTipoCategoriaTituloBL.carregar(UtilNumber.toInt32(id)) ?? new TipoCategoriaTitulo();

			ViewModel.TipoCategoriaTitulo = OTipoCategoriaTitulo;
		    ViewModel.TipoCategoriaTitulo.Categoria = ViewModel.TipoCategoriaTitulo.Categoria ?? new CategoriaTitulo();

			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(TipoCategoriaTituloForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OTipoCategoriaTituloBL.salvar(ViewModel.TipoCategoriaTitulo);
			
            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.TipoCategoriaTitulo.id });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				bool flagSucesso = this.OTipoCategoriaTituloBL.excluir(idExclusao);

				if (!flagSucesso) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}

			return Json(Retorno);
		}

        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {
            return Json(this.OTipoCategoriaTituloBL.alterarStatus(id));
        }

        [ActionName("listar-ajax")]
        public ActionResult listarAjax(int? idCategoria) {

			var query = this.OTipoCategoriaTituloBL.listar(0, UtilNumber.toInt32(idCategoria), "","S");
			var lista = query.Select(x => new { value = x.id, text = x.descricao }).Distinct().OrderBy(x => x.text).ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
	}
}