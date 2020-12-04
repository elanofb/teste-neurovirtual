using System;
using System.Linq;
using System.Web.Mvc;
using BLL.PedidosTemp;
using DAL.PedidosTemp;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoCadastroProdutoController : Controller {

        //Constantes

        //Atributos
        private IPedidoTempBL _IPedidoTempBL;
        private IPedidoProdutoTempBL _IPedidoProdutoTempBL;

        //Propriedades
        private IPedidoTempBL OPedidoTempBL => _IPedidoTempBL = _IPedidoTempBL ?? new PedidoTempBL();
        private IPedidoProdutoTempBL OPedidoProdutoTempBL => _IPedidoProdutoTempBL = _IPedidoProdutoTempBL ?? new PedidoProdutoTempBL();

        //Adicionar um produto ao pedido
        //Resposta para função ajax
        [HttpPost, ActionName("adicionar-produto")]
        public ActionResult adicionarProduto() {

            var ViewModel = new PedidoCadastroProdutoInclusaoVM();

            var ORetorno = ViewModel.adicionarProduto();

            if (ORetorno.flagError) {
                
                return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
                    
            }

            return Json(new { error = false, message = "Produto adicionado/atualizado com sucesso" }, JsonRequestBehavior.AllowGet);

        }

        //Excluir produto adicionado ao pedido
		//Resposta de função Ajax
        [HttpPost, ActionName("remover-produto")]
        public ActionResult removerProduto() {

            int idProduto = UtilRequest.getInt32("idProduto");

            var listaPedidoProduto = this.OPedidoTempBL.carregar(Session.SessionID)?.listaProdutos;
            
            var OPedidoItem = listaPedidoProduto?.FirstOrDefault(x => x.idProduto == idProduto) ?? new PedidoProdutoTemp();

            this.OPedidoProdutoTempBL.excluir(OPedidoItem.id);

            return Json(new { error = false, message = "Produto removido com sucesso do pedido." }, JsonRequestBehavior.AllowGet);

        }

        // Listar somente o bloco dos produtos do pedido
        [ActionName("partial-produtos")]
        public PartialViewResult partialProdutos() {

            var ViewModel = new PedidoCadastroForm();

            ViewModel.Pedido = this.OPedidoTempBL.carregar(Session.SessionID);
            
            return PartialView(ViewModel);
        }


    }
}