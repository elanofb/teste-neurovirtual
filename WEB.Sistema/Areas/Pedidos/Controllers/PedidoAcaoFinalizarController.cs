using System;
using System.Web.Mvc;
using BLL.Pedidos;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoAcaoFinalizacaoController : Controller {

        //Constantes

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoAcaoFinalizacaoBL _IPedidoAcaoFinalizacaoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => this._IPedidoBL = this._IPedidoBL ?? new PedidoBL();
        private IPedidoAcaoFinalizacaoBL OPedidoAcaoFinalizacaoBL => _IPedidoAcaoFinalizacaoBL = _IPedidoAcaoFinalizacaoBL ?? new PedidoAcaoFinalizacaoBL();

        //
        [HttpGet, ActionName("modal-finalizar-pedido")]
        public ActionResult modalFinalizarPedido(int? id) {
            
            var OPedido = this.OPedidoBL.carregar(id.toInt());

            if (OPedido == null) {
                return Json(new { flagError = true, message = "O pedido informado não foi encontrado." });
            }

            if (OPedido.dtFinalizado.HasValue) {
                return Json(new { flagError = true, message = $"O pedido informado já foi finalizado em { OPedido.dtFinalizado.exibirData() }." });
            }

            var ViewModel = new PedidoAcaoForm();
            
            ViewModel.id = OPedido.id;
            
            return View(ViewModel);

        }

        //
        [HttpPost]
        public ActionResult finalizar(PedidoAcaoForm ViewModel) {
            
            if (!ModelState.IsValid) {
                
                return View("modal-finalizar-pedido", ViewModel);
            }

            this.OPedidoAcaoFinalizacaoBL.finalizar(ViewModel.id, ViewModel.observacoes);

            return Json(new { error = false }, JsonRequestBehavior.AllowGet);    
            
        }
        

    }
}