using System;
using System.Web.Mvc;
using BLL.Pedidos;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoAcaoCancelamentoController : Controller {

        //Constantes

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoAcaoCancelamentoBL _IPedidoAcaoCancelamentoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => this._IPedidoBL = this._IPedidoBL ?? new PedidoBL();
        private IPedidoAcaoCancelamentoBL OPedidoAcaoCancelamentoBL => _IPedidoAcaoCancelamentoBL = _IPedidoAcaoCancelamentoBL ?? new PedidoAcaoCancelamentoBL();

        //
        [HttpGet, ActionName("modal-cancelar-pedido")]
        public ActionResult modalCancelarPedido(int? id) {
            
            var OPedido = this.OPedidoBL.carregar(id.toInt());

            if (OPedido == null) {
                return Json(new { flagError = true, message = "O pedido informado não foi encontrado." });
            }

            if (OPedido.dtCancelamento.HasValue) {
                return Json(new { flagError = true, message = $"O pedido informado já foi cancelado em { OPedido.dtCancelamento.exibirData() }." });
            }

            var ViewModel = new PedidoAcaoCancelamentoForm();
            
            ViewModel.id = OPedido.id;
            
            return View(ViewModel);

        }

        //
        [HttpPost]
        public ActionResult cancelar(PedidoAcaoCancelamentoForm ViewModel) {
            
            if (!ModelState.IsValid) {
                
                return View("modal-cancelar-pedido", ViewModel);
            }

            this.OPedidoAcaoCancelamentoBL.cancelar(ViewModel.id, ViewModel.motivoCancelamento);

            return Json(new { error = false }, JsonRequestBehavior.AllowGet);    
            
        }
        

    }
}