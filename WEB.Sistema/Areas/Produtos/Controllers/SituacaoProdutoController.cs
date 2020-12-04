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
	public class SituacaoProdutoController : Controller {

		//Constantes

		//Atributos
		private IProdutoSituacaoConsultaBL _IProdutoSituacaoConsultaBL;
		private IProdutoSituacaoCadastroBL _IProdutoSituacaoCadastroBL;
		private IProdutoSituacaoExclusaoBL _IProdutoSituacaoExclusaoBL;

		//Propriedades
		private IProdutoSituacaoConsultaBL OProdutoSituacaoConsultaBL => _IProdutoSituacaoConsultaBL = _IProdutoSituacaoConsultaBL ?? new ProdutoSituacaoConsultaBL();
		private IProdutoSituacaoCadastroBL OProdutoSituacaoCadastroBL => _IProdutoSituacaoCadastroBL = _IProdutoSituacaoCadastroBL ?? new ProdutoSituacaoCadastroBL();
		private IProdutoSituacaoExclusaoBL OProdutoSituacaoExclusaoBL => _IProdutoSituacaoExclusaoBL = _IProdutoSituacaoExclusaoBL ?? new ProdutoSituacaoExclusaoBL();

		//
		public SituacaoProdutoController() { 
		} 

		//
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

			bool? ativo = UtilRequest.getBool("flagAtivo");

			var listaProdutoSituacao = this.OProdutoSituacaoConsultaBL.listar(descricao, ativo).OrderBy(x => x.id);

			return View(listaProdutoSituacao.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new ProdutoSituacaoForm();

		    ViewModel.ProdutoSituacao = this.OProdutoSituacaoConsultaBL.carregar(UtilNumber.toInt32(id)) ?? new ProdutoSituacao();
            
			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(ProdutoSituacaoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			if (ViewModel.ProdutoSituacao.id == 0)
			{
				ViewModel.ProdutoSituacao.ativo = true;
				ViewModel.ProdutoSituacao.dtCadastro = DateTime.Now;
			}
			else
			{
				ViewModel.ProdutoSituacao.dtAlteracao = DateTime.Now;
			}

			bool flagSucesso = this.OProdutoSituacaoCadastroBL.salvar(ViewModel.ProdutoSituacao);
			
            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.ProdutoSituacao.id });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));
            
			return View(ViewModel);

		}
        
	    //
	    [HttpPost, ActionName("alterar-status")]
	    public ActionResult alterarStatus(int id) {
	        return Json(this.OProdutoSituacaoCadastroBL.alterarStatus(id));
	    }

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {

		    var Retorno = new JsonMessage();
		    Retorno.error = false;
		    Retorno.message = "Os registros informados foram removidos com sucesso.";

		    foreach (int idExclusao in id) {

		        var ORetornoExclusao = this.OProdutoSituacaoExclusaoBL.excluir(idExclusao);

		        if (ORetornoExclusao.flagError) {
		            Retorno.error = true;
		            Retorno.message = ORetornoExclusao.listaErros.FirstOrDefault();
		        }

		    }

		    return Json(Retorno);
		}

	}
}