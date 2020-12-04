using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;
using BLL.Services;
using DAL.Pedidos;
using MvcFlashMessages;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoDetalhesProdutoController : Controller {

        //Constantes

        //Atributos
        private IPedidoProdutoBL _IPedidoProdutoBL;
        private IPedidoBL _PedidoBL;
        private IPedidoProdutoOperacaoBL _IPedidoProdutoOperacaoBL;

        //Propriedades
        private IPedidoProdutoBL OPedidoProdutoBL => _IPedidoProdutoBL = _IPedidoProdutoBL ?? new PedidoProdutoBL();
        private IPedidoBL OPedidoBL => _PedidoBL = _PedidoBL ?? new PedidoBL();
        private IPedidoProdutoOperacaoBL OPedidoProdutoOperacaoBL => _IPedidoProdutoOperacaoBL = _IPedidoProdutoOperacaoBL ?? new PedidoProdutoOperacaoBL();

        // 
        [ActionName("partial-lista-produtos")]
        public PartialViewResult partialListaProdutos(int idPedido) {

            var ViewModel = new PedidoDetalhesProdutoVM();

            ViewModel.idPedido = idPedido;

            ViewModel.listaProdutos = OPedidoProdutoBL.listar(idPedido)
                                                .Where(x => x.flagExcluido == "N")
                                                      .Select(x => new { x.id, 
                                                                           x.idProduto, 
                                                                           x.nomeProduto, 
                                                                           x.qtde, 
                                                                           x.peso, 
                                                                           x.valorItem, 
                                                                           x.observacoes,
                                                                           x.dtFimGanhoDiario,
                                                                           Pedido = new {
                                                                                            x.Pedido.id,
                                                                                            x.Pedido.dtQuitacao
                                                                                            
                                                                                        },
                                                                           Produto = new {
                                                                                             x.Produto.id,
                                                                                             x.Produto.valorGanhoDiario,
                                                                                             x.Produto.qtdeDiasDuracao,
                                                                                             x.Produto.qtdePontosPlanoCarreira
                                                                                         }
                                                                       }
                                                      )
                                                      .ToListJsonObject<PedidoProduto>();

            return PartialView(ViewModel);

        }

        //
        [ActionName("modal-adicionar-produto")]
        public ActionResult modalAdicionarProduto(int idPedido) {

            var ViewModel = new PedidoDetalhesProdutoForm();

            ViewModel.OPedidoProduto = new PedidoProduto() { idPedido = idPedido, qtde = 1 };

            return View(ViewModel);

        }

        //Adicionar um produto ao pedido
        [HttpPost]
        public ActionResult salvar(PedidoDetalhesProdutoForm ViewModel) {

            var OPedido = this.OPedidoBL.carregar(ViewModel.OPedidoProduto.idPedido);

            if (OPedido.dtFaturamento.HasValue || OPedido.dtCancelamento.HasValue || OPedido.dtQuitacao.HasValue) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Operação Inválida!", "Não é possível alterar os itens do pedido após faturamento, quitação ou cancelamento."));

                return View("modal-adicionar-produto", ViewModel);
            }

            if (!ModelState.IsValid) {
                return View("modal-adicionar-produto", ViewModel);
            }

            this.OPedidoProdutoOperacaoBL.adicionar(ViewModel.OPedidoProduto);

            return Json(new { error = false, message = "Produto adicionado/atualizado com sucesso" }, JsonRequestBehavior.AllowGet);

        }

        //Excluir produto adicionado ao pedido
        //Resposta de função Ajax
        [HttpPost, ActionName("remover-produto")]
        public ActionResult removerProduto(int id) {

            var OPedidoProduto = this.OPedidoProdutoBL.carregar(id);

            var OPedido = OPedidoProduto.Pedido;

            if (OPedido.dtFaturamento.HasValue || OPedido.dtCancelamento.HasValue || OPedido.dtQuitacao.HasValue) {

                return Json(new { error = true, message = "Não é possível alterar os itens do pedido após faturamento, quitação ou cancelamento." }, JsonRequestBehavior.AllowGet);

            }

            var ORetorno = this.OPedidoProdutoOperacaoBL.excluir(id);

            if (ORetorno.flagError) {

                return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { error = false, message = "Produto removido com sucesso do pedido." }, JsonRequestBehavior.AllowGet);

        }

    }

}