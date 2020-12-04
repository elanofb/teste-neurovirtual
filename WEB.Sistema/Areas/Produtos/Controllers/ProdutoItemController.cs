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
	public class ProdutoItemController : Controller {

		//Constantes

		//Atributos
		private IProdutoItemConsultaBL _IProdutoItemConsultaBL;
		private IProdutoItemCadastroBL _IProdutoItemCadastroBL;
		private IProdutoItemExclusaoBL _IProdutoItemExclusaoBL;

		//Propriedades
		private IProdutoItemConsultaBL OProdutoItemConsultaBL => _IProdutoItemConsultaBL = _IProdutoItemConsultaBL ?? new ProdutoItemConsultaBL();
		private IProdutoItemCadastroBL OProdutoItemCadastroBL => _IProdutoItemCadastroBL = _IProdutoItemCadastroBL ?? new ProdutoItemCadastroBL();
		private IProdutoItemExclusaoBL OProdutoItemExclusaoBL => _IProdutoItemExclusaoBL = _IProdutoItemExclusaoBL ?? new ProdutoItemExclusaoBL();

		//
		public ProdutoItemController() { 
		} 

		//
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

			bool? ativo = UtilRequest.getBool("flagAtivo");

			var listaProdutoItem = this.OProdutoItemConsultaBL.listar(descricao, ativo).OrderBy(x => x.dtCadastro);

			return View(listaProdutoItem.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new ProdutoItemForm();

		    ViewModel.ProdutoItem = this.OProdutoItemConsultaBL.carregar(UtilNumber.toInt32(id)) ?? new ProdutoItem();
            
			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(ProdutoItemForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OProdutoItemCadastroBL.salvar(ViewModel.ProdutoItem);
			
            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.ProdutoItem.id });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));
            
			return View(ViewModel);

		}
        
	    //
	    [HttpPost, ActionName("alterar-status")]
	    public ActionResult alterarStatus(int id) {
	        return Json(this.OProdutoItemCadastroBL.alterarStatus(id));
	    }

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {

		    var Retorno = new JsonMessage();
		    Retorno.error = false;
		    Retorno.message = "Os registros informados foram removidos com sucesso.";

		    foreach (int idExclusao in id) {

		        var ORetornoExclusao = this.OProdutoItemExclusaoBL.excluir(idExclusao);

		        if (ORetornoExclusao.flagError) {
		            Retorno.error = true;
		            Retorno.message = ORetornoExclusao.listaErros.FirstOrDefault();
		        }

		    }

		    return Json(Retorno);
		}
	}
}