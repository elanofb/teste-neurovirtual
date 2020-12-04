using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoCobrancaController : Controller {

        //Atributos
        private IPedidoCobrancaBL _PedidoCobrancaBL;

        //Propriedades
        private IPedidoCobrancaBL OPedidoCobrancaBL => _PedidoCobrancaBL = _PedidoCobrancaBL ?? new PedidoCobrancaBL();

        // Pedidos/PedidoCobranca/enviar-email-cobranca
        [ActionName("enviar-email-cobranca")]
        public ActionResult enviarEmailCobranca() {

            int idPedido = UtilRequest.getInt32("idPedido");

            var Retorno = this.OPedidoCobrancaBL.enviarEmailCobranca(idPedido);

            return Json(new {error = Retorno.flagError, message = Retorno.listaErros.FirstOrDefault()});
        }
    }
}