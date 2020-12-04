using System;
using System.Web.Mvc;
using BLL.Pedidos;
using MvcFlashMessages;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoAcaoFaturamentoController : Controller {

        //Constantes

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoAcaoFaturamentoBL _IPedidoAcaoFaturamentoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();
        private IPedidoAcaoFaturamentoBL OPedidoAcaoFaturamentoBL => _IPedidoAcaoFaturamentoBL = _IPedidoAcaoFaturamentoBL ?? new PedidoAcaoFaturamentoBL();

        //
        [HttpGet, ActionName("modal-faturar-pedido")]
        public ActionResult modalFaturarPedido(int? id) {
            
            var OPedido = this.OPedidoBL.carregar(id.toInt());

            if (OPedido == null) {
                return Json(new { flagError = true, message = "O pedido informado não foi encontrado." });
            }

            if (OPedido.dtFaturamento.HasValue) {
                return Json(new { flagError = true, message = $"O pedido informado já foi faturado em { OPedido.dtFaturamento.exibirData() }." });
            }

            var ViewModel = new PedidoAcaoFaturamentoForm();
            
            ViewModel.Pedido = OPedido;
            
            return View(ViewModel);

        }

        //
        [HttpPost]
        public ActionResult faturar(PedidoAcaoFaturamentoForm ViewModel) {
            
            if (ViewModel.Pedido.flagCartaoCreditoPermitido != true &&
                ViewModel.Pedido.flagBoletoBancarioPermitido != true &&
                ViewModel.Pedido.flagDepositoPermitido != true) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Você deve habilitar pelo menos uma forma de pagamento.");

                return View("modal-faturar-pedido", ViewModel);

            }

            this.OPedidoAcaoFaturamentoBL.salvarDadosFaturamento(ViewModel.Pedido);

            return Json(new { error = false }, JsonRequestBehavior.AllowGet);    
            
        }
        
    }

}