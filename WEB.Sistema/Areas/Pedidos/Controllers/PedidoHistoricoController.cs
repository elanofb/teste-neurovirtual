using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoHistoricoController : Controller {

        //Constantes

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoHistoricoBL _PedidoHistoricoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => this._IPedidoBL = this._IPedidoBL ?? new PedidoBL();
        private IPedidoHistoricoBL OPedidoHistoricoBL => this._PedidoHistoricoBL = this._PedidoHistoricoBL ?? new PedidoHistoricoBL();


        //
        [ActionName("partial-historico-pedido")]
        public ActionResult partialHistoricoPedido(int idPedido) {

            var listaOcorrencias = this.OPedidoHistoricoBL.listar(idPedido)
                                                          .OrderByDescending(x => x.id)
                                                          .ToList();

            return PartialView(listaOcorrencias);
        }


    }
}