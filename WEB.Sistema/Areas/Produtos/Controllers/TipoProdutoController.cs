using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;
using DAL.Produtos;
using PagedList;
using WEB.Areas.Produtos.ViewModels;
using System.Json;
using MvcFlashMessages;

namespace WEB.Areas.Produtos.Controllers {

    [OrganizacaoFilter]
	public class TipoProdutoController : Controller {

		//Constantes

		//Atributos
		private ITipoProdutoBL _ITipoProdutoBL;

		//Propriedades
		private ITipoProdutoBL OTipoProdutoBL => _ITipoProdutoBL = _ITipoProdutoBL ?? new TipoProdutoBL();

		//
		public TipoProdutoController() { 
		} 

		//
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

			bool? ativo = UtilRequest.getBool("flagAtivo");

			var listaTipoProduto = this.OTipoProdutoBL.listar(descricao, ativo).OrderBy(x => x.descricao);

			return View(listaTipoProduto.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new TipoProdutoForm();

		    ViewModel.TipoProduto = this.OTipoProdutoBL.carregar(UtilNumber.toInt32(id)) ?? new TipoProduto();
            
			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(TipoProdutoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OTipoProdutoBL.salvar(ViewModel.TipoProduto);
			
            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.TipoProduto.id });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));
            
			return View(ViewModel);

		}

		[HttpGet, ActionName("modal-editar")]
		public ActionResult modalEditar(int? id) {
			
			var ViewModel = new TipoProdutoForm();
			
			ViewModel.TipoProduto = this.OTipoProdutoBL.carregar(UtilNumber.toInt32(id)) ?? new TipoProduto();

			return PartialView(ViewModel);
		}

		[HttpPost, ActionName("salvar-modal-editar")]
		public ActionResult salvarModalEditar(TipoProdutoForm ViewModel) {

			if (!ModelState.IsValid) {
				return PartialView("modal-editar", ViewModel);
			}

			bool flagSucesso = this.OTipoProdutoBL.salvar(ViewModel.TipoProduto);

			return Json(new {error = false, flagSucesso, ViewModel.TipoProduto.id, ViewModel.TipoProduto.descricao});
		}

	    //
	    [HttpPost, ActionName("alterar-status")]
	    public ActionResult alterarStatus(int id) {
	        return Json(this.OTipoProdutoBL.alterarStatus(id));
	    }

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {

		    var Retorno = new JsonMessage();
		    Retorno.error = false;
		    Retorno.message = "Os registros informados foram removidos com sucesso.";

		    foreach (int idExclusao in id) {

		        var ORetornoExclusao = this.OTipoProdutoBL.excluir(idExclusao);

		        if (ORetornoExclusao.flagError) {
		            Retorno.error = true;
		            Retorno.message = ORetornoExclusao.listaErros.FirstOrDefault();
		        }

		    }

		    return Json(Retorno);
		}
	}
}