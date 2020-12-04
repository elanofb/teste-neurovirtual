using System;
using System.Web.Mvc;
using BLL.Pedidos;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoAcaoPreparacaoController : Controller {

        //Constantes

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoAcaoPreparacaoBL _IPedidoAcaoPreparacaoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => this._IPedidoBL = this._IPedidoBL ?? new PedidoBL();
        private IPedidoAcaoPreparacaoBL OPedidoAcaoPreparacaoBL => _IPedidoAcaoPreparacaoBL = _IPedidoAcaoPreparacaoBL ?? new PedidoAcaoPreparacaoBL();

        //
        [HttpGet, ActionName("modal-preparar-pedido")]
        public ActionResult modalPrepararPedido(int? id) {
            
            var OPedido = this.OPedidoBL.carregar(id.toInt());

            if (OPedido == null) {
                return Json(new { flagError = true, message = "O pedido informado não foi encontrado." });
            }

            if (OPedido.dtPreparacao.HasValue) {
                return Json(new { flagError = true, message = $"O pedido informado já foi preparado em { OPedido.dtPreparacao.exibirData() }." });
            }

            var ViewModel = new PedidoAcaoForm();
            
            ViewModel.id = OPedido.id;
            
            return View(ViewModel);

        }

        //
        [HttpPost]
        public ActionResult preparar(PedidoAcaoForm ViewModel) {
            
            if (!ModelState.IsValid) {
                
                return View("modal-preparar-pedido", ViewModel);
            }

            this.OPedidoAcaoPreparacaoBL.preparar(ViewModel.id, ViewModel.observacoes);

            return Json(new { error = false }, JsonRequestBehavior.AllowGet);    
            
        }
        

    }
}