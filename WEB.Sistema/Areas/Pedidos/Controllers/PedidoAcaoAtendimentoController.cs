using System;
using System.Web.Mvc;
using BLL.Pedidos;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoAcaoAtendimentoController : Controller {

        //Constantes

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoAcaoAtendimentoBL _IPedidoAcaoAtendimentoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => this._IPedidoBL = this._IPedidoBL ?? new PedidoBL();
        private IPedidoAcaoAtendimentoBL OPedidoAcaoAtendimentoBL => _IPedidoAcaoAtendimentoBL = _IPedidoAcaoAtendimentoBL ?? new PedidoAcaoAtendimentoBL();

        //
        [HttpGet, ActionName("modal-atender-pedido")]
        public ActionResult modalAtender(int? id) {
            
            var OPedido = this.OPedidoBL.carregar(id.toInt());

            if (OPedido == null) {
                return Json(new { flagError = true, message = "O pedido informado não foi encontrado." });
            }

            if (OPedido.dtAtendimento.HasValue) {
                return Json(new { flagError = true, message = $"O pedido informado já foi atendido em { OPedido.dtAtendimento.exibirData() }." });
            }

            var ViewModel = new PedidoAcaoForm();
            
            ViewModel.id = OPedido.id;
            
            return View(ViewModel);

        }

        //
        [HttpPost]
        public ActionResult atender(PedidoAcaoForm ViewModel) {
            
            if (!ModelState.IsValid) {
                
                return View("modal-atender-pedido", ViewModel);
            }

            this.OPedidoAcaoAtendimentoBL.atender(ViewModel.id, ViewModel.observacoes);

            return Json(new { error = false }, JsonRequestBehavior.AllowGet);    
            
        }
        

    }
}