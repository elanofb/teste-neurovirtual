using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoWidgetController : Controller {

        //Constantes

        //Atributos
        private IPedidoBL _IPedidoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();
       
        public PedidoWidgetController() {

        }


        [ActionName("widget-ultimos-pedidos")]
        public PartialViewResult listarUltimosPedidos() {

            var listaPedidos = this.OPedidoBL.listar("", "S", 0).OrderByDescending(x => x.id).Take(10).ToList();

            return PartialView(listaPedidos);

        }

    }
}