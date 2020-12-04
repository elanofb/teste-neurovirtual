using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;
using DAL.Produtos;
using PagedList;
using WEB.Areas.Produtos.ViewModels;
using System.Json;
using BLL.Funcionarios;
using System.Web.UI.WebControls;
using MvcFlashMessages;

namespace WEB.Areas.Produtos.Controllers {

    public class EstoqueSaidaController : Controller {

		//Constantes

		//Atributos
        private EstoqueSaidaBL _EstoqueSaidaBL;
        private FuncionarioConsultaBL _FuncionarioConsultaBL;

		//Propriedades
        private FuncionarioConsultaBL OFuncionarioConsultaBL{ get{ return ( this._FuncionarioConsultaBL = this._FuncionarioConsultaBL ?? new FuncionarioConsultaBL() ); } }
        private EstoqueSaidaBL OEstoqueSaidaBL{ get{ return (this._EstoqueSaidaBL = this._EstoqueSaidaBL ?? new EstoqueSaidaBL() ); } }
		
		//
		public EstoqueSaidaController() { 
		}
	
		//
        public ActionResult listar() {
			string descricao = UtilRequest.getString("valorBusca");
			string ativo = UtilRequest.getString("flagAtivo");
			int idTipoReferencia = UtilRequest.getInt32("idTipoReferencia");
			int idReferencia = UtilRequest.getInt32("idReferencia");
            int idProduto = UtilRequest.getInt32("idProduto");

            var listaTipoEstoqueSaida = this.OEstoqueSaidaBL.listar(idTipoReferencia, idReferencia, idProduto, descricao, ativo)
										.OrderBy(x => x.ProdutoEstoque.Produto.nome);
            
            return View(listaTipoEstoqueSaida.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

		//Carregamento de formulario para edicao ou inclusao de novo registro
		[HttpGet]
        public ActionResult editar(int? id) {

            EstoqueSaidaForm ViewModel = new EstoqueSaidaForm();

            var OEstoqueSaida = this.OEstoqueSaidaBL.carregar(UtilNumber.toInt32(id)) ?? new EstoqueSaida();


			ViewModel.EstoqueSaida = OEstoqueSaida;

			return View(ViewModel);
        }

        //
		[HttpPost]
        public ActionResult editar(EstoqueSaidaForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

            bool flagSucesso = this.OEstoqueSaidaBL.salvar(ViewModel.EstoqueSaida);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }

            if (ViewModel.EstoqueSaida.id > 0) { 

                return RedirectToAction("editar", new { id = ViewModel.EstoqueSaida.id });

            }

			return View(ViewModel);
        }
	
		//
        [HttpPost]
        public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				bool flagSucesso = this.OEstoqueSaidaBL.excluir(idExclusao);

				if (!flagSucesso) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}

			return Json(Retorno);
        }

        [ActionName("carregar-referencias")]
        public ActionResult carregarReferencias(int? idTipoReferenciaSaida) {

            switch (UtilNumber.toInt32(idTipoReferenciaSaida)) { 

                case (int)TipoReferenciaSaidaEnum.FUNCIONARIOS :

                    var listFuncionarios = OFuncionarioConsultaBL.listar("", "S").OrderBy(x => x.Pessoa.nome).Select(x => new { id = x.id, nome = x.Pessoa.nome }).ToList();
                    return Json(listFuncionarios, JsonRequestBehavior.AllowGet);

                case (int)TipoReferenciaSaidaEnum.OUTROS :

                    var list = new[] { new { id = "9999", nome = "Outros" } };
                    return Json(list, JsonRequestBehavior.AllowGet);
            }

            var listDefault = new[] { new { id = "", nome = "..." } };
            return Json(listDefault, JsonRequestBehavior.AllowGet);
        }

        [ActionName("gerar-excel"),HttpPost]
        public ActionResult gerarExcel() {

            var ids = UtilRequest.getListInt("ids");
            var listaEstoqueSaida = this.OEstoqueSaidaBL.listarPorId(ids).ToList();

            if(listaEstoqueSaida.Count > 0) {

                var listaExcel = listaEstoqueSaida.Select(x => new {
                    x.id,
                    x.ProdutoEstoque.dtMovimentacao,
                    tipoSaida = x.TipoReferenciaSaida.descricao,
                    saidaPara = ((x.idTipoReferenciaSaida == (int)TipoReferenciaSaidaEnum.FUNCIONARIOS) ?
                                 this.OFuncionarioConsultaBL.carregar(x.idReferencia).Pessoa.nome :
                                 "Outros"),
                    produto = x.ProdutoEstoque.Produto.nome,
                    quantidade = x.ProdutoEstoque.qtdMovimentada,
                    descricao = x.ProdutoEstoque.descricao,
                    dtCadastro = UtilDate.toDisplay(x.dtCadastro.ToString()),
                    status = (x.ativo == "S") ? "Sim" : "Não",
                }).ToList();

                var OGrid = new GridView();
                OGrid.DataSource = listaExcel;
                OGrid.DataBind();

                OGrid.HeaderRow.Cells[0].Text = "ID";
                OGrid.HeaderRow.Cells[1].Text = "Data Entrada";
                OGrid.HeaderRow.Cells[2].Text = "Tipo Saída";
                OGrid.HeaderRow.Cells[3].Text = "Saída Para";
                OGrid.HeaderRow.Cells[4].Text = "Produto";
                OGrid.HeaderRow.Cells[5].Text = "Quantidade";
                OGrid.HeaderRow.Cells[6].Text = "Descriç&atilde;o";
                OGrid.HeaderRow.Cells[7].Text = "Data de Cadastro";
                OGrid.HeaderRow.Cells[8].Text = "Ativo";

                UTIL.Excel.UtilExcel OExcel = new UTIL.Excel.UtilExcel();
                OExcel.downloadExcel(Response,OGrid,String.Concat("Lista de Saída do Estoque - ",DateTime.Now.ToShortDateString().Replace("/","-"),".xls"));
            }

            return null;
        }
	}
}