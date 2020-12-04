using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoCancelamentoController : Controller {

        //Constantes

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoCancelamentoBL _IPedidoCancelamentoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => this._IPedidoBL = this._IPedidoBL ?? new PedidoBL();
        private IPedidoCancelamentoBL OPedidoCancelamentoBL => _IPedidoCancelamentoBL = _IPedidoCancelamentoBL ?? new PedidoCancelamentoBL();

        //
        [ActionName("modal-cancelar-pedido")]
        public ActionResult modalCancelarPedido(int[] ids) {
            
            var listaPedidos = this.OPedidoBL.listar("", "", 0).Where(x => ids.Contains(x.id))
                                   .Select(x => new { x.id, x.dtCancelamento }).ToList();

            if (!listaPedidos.Any()) {
                return Json(new { flagError = true, message = "O pedido informado não foi encontrado." });
            }

            if (listaPedidos.Any(x => x.dtCancelamento.HasValue)) {
                return Json(new { flagError = true, message = $"Um ou mais pedidos informados já foram cancelados anteriormente." });
            }

            var ViewModel = new PedidoCancelamentoForm();
            
            ViewModel.ids = ids;
            
            return View(ViewModel);

        }

        //
        [HttpPost]
        public ActionResult cancelar(PedidoCancelamentoForm ViewModel) {
            
            if (!ModelState.IsValid) {
                return View("modal-cancelar-pedido", ViewModel);
            }

            this.OPedidoCancelamentoBL.cancelar(ViewModel.ids, ViewModel.motivoCancelamento);

            return Json(new { error = false }, JsonRequestBehavior.AllowGet);    
            
        }
        

    }
}