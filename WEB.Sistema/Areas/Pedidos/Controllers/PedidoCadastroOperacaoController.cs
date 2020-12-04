using System.Web.Mvc;
using BLL.PedidosTemp;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoCadastroOperacaoController : Controller {

        //Constantes

        //Atributos
        private IPedidoTempOperacaoBL _IPedidoTempOperacaoBL;

        //Propriedades
        private IPedidoTempOperacaoBL OPedidoTempOperacaoBL => _IPedidoTempOperacaoBL = _IPedidoTempOperacaoBL ?? new PedidoTempOperacaoBL();

        //Inicio de um novo pedido ou visualização de um pedido já realizado
        [ActionName("novo-pedido")]
        public ActionResult novoPedido() {

            this.OPedidoTempOperacaoBL.limpar(Session.SessionID);
   
            return RedirectToAction("index", "PedidoCadastro");

        }
        
    }

}