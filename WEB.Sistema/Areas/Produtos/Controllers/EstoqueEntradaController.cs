using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;
using DAL.Produtos;
using PagedList;
using WEB.Areas.Produtos.ViewModels;
using System.Json;
using System.Web.UI.WebControls;
using MvcFlashMessages;

namespace WEB.Areas.Produtos.Controllers {

    public class EstoqueEntradaController : Controller {

		//Constantes

		//Atributos
        private EstoqueEntradaBL _EstoqueEntradaBL;

		//Propriedades
        private EstoqueEntradaBL OEstoqueEntradaBL{ get{ return (this._EstoqueEntradaBL = this._EstoqueEntradaBL ?? new EstoqueEntradaBL() ); } }
		
		//
		public EstoqueEntradaController() { 
		}
	
		//
        public ActionResult listar() {
			string descricao = UtilRequest.getString("valorBusca");
			string ativo = UtilRequest.getString("flagAtivo");
			int idFornecedor = UtilRequest.getInt32("idFornecedor");
			int idProduto = UtilRequest.getInt32("idProduto");

            var listaTipoEstoqueEntrada = this.OEstoqueEntradaBL.listar(idFornecedor, idProduto, descricao, ativo)
										.OrderByDescending(x => x.ProdutoEstoque.dtMovimentacao);
            
            return View(listaTipoEstoqueEntrada.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

		//Carregamento de formulario para edicao ou inclusao de novo registro
		[HttpGet]
        public ActionResult editar(int? id) {

            EstoqueEntradaForm ViewModel = new EstoqueEntradaForm();

            var OEstoqueEntrada = this.OEstoqueEntradaBL.carregar(UtilNumber.toInt32(id)) ?? new EstoqueEntrada();

			ViewModel.EstoqueEntrada = OEstoqueEntrada;

			return View(ViewModel);
        }

        //
		[HttpPost]
        public ActionResult editar(EstoqueEntradaForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

            bool flagSucesso = this.OEstoqueEntradaBL.salvar(ViewModel.EstoqueEntrada);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }

            if (ViewModel.EstoqueEntrada.id > 0) { 
                return RedirectToAction("editar", new { id = ViewModel.EstoqueEntrada.id });
            }

			return View(ViewModel);
        }    
	
		//
        [HttpPost]
        public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				bool flagSucesso = this.OEstoqueEntradaBL.excluir(idExclusao);

				if (!flagSucesso) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}

			return Json(Retorno);
        }

        [ActionName("gerar-excel"),HttpPost]
        public ActionResult gerarExcel() {

            var ids = UtilRequest.getListInt("ids");
            var listaEstoqueEntrada = this.OEstoqueEntradaBL.listarPorId(ids).ToList();

            if(listaEstoqueEntrada.Count > 0) {

                var listaExcel = listaEstoqueEntrada.Select(x => new {
                    x.id,
                    x.ProdutoEstoque.dtMovimentacao,
                    fornecedor = x.Fornecedor.Pessoa.nome,
                    produto = x.ProdutoEstoque.Produto.nome,
                    quantidade = x.ProdutoEstoque.qtdMovimentada,
                    dtCadastro = UtilDate.toDisplay(x.dtCadastro.ToString()),
                    status = (x.ativo == "S") ? "Sim" : "Não",
                }).ToList();

                var OGrid = new GridView();
                OGrid.DataSource = listaExcel;
                OGrid.DataBind();

                OGrid.HeaderRow.Cells[0].Text = "ID";
                OGrid.HeaderRow.Cells[1].Text = "Data Entrada";
                OGrid.HeaderRow.Cells[2].Text = "Fornecedor";
                OGrid.HeaderRow.Cells[3].Text = "Produto";
                OGrid.HeaderRow.Cells[4].Text = "Quantidade";
                OGrid.HeaderRow.Cells[5].Text = "Data de Cadastro";
                OGrid.HeaderRow.Cells[6].Text = "Ativo";

                UTIL.Excel.UtilExcel OExcel = new UTIL.Excel.UtilExcel();
                OExcel.downloadExcel(Response,OGrid,String.Concat("Lista de Entrada do Estoque - ",DateTime.Now.ToShortDateString().Replace("/","-"),".xls"));
            }

            return null;
        }
	}
}