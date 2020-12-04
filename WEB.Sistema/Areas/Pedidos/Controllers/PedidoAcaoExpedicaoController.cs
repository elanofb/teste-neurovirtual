using System;
using System.Web.Mvc;
using BLL.Pedidos;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoAcaoExpedicaoController : Controller {

        //Constantes

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoAcaoExpedicaoBL _IPedidoAcaoExpedicaoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => this._IPedidoBL = this._IPedidoBL ?? new PedidoBL();
        private IPedidoAcaoExpedicaoBL OPedidoAcaoExpedicaoBL => _IPedidoAcaoExpedicaoBL = _IPedidoAcaoExpedicaoBL ?? new PedidoAcaoExpedicaoBL();

        //
        [HttpGet, ActionName("modal-expedir-pedido")]
        public ActionResult modalExpedirPedido(int? id) {
            
            var OPedido = this.OPedidoBL.carregar(id.toInt());

            if (OPedido == null) {
                return Json(new { flagError = true, message = "O pedido informado não foi encontrado." });
            }

            if (OPedido.dtExpedicao.HasValue) {
                return Json(new { flagError = true, message = $"O pedido informado já foi expedido em { OPedido.dtExpedicao.exibirData() }." });
            }

            var ViewModel = new PedidoAcaoForm();
            
            ViewModel.id = OPedido.id;
            
            return View(ViewModel);

        }

        //
        [HttpPost]
        public ActionResult expedir(PedidoAcaoForm ViewModel) {
            
            if (!ModelState.IsValid) {
                
                return View("modal-expedir-pedido", ViewModel);
            }

            this.OPedidoAcaoExpedicaoBL.expedir(ViewModel.id, ViewModel.observacoes);

            return Json(new { error = false }, JsonRequestBehavior.AllowGet);    
            
        }
        

    }
}