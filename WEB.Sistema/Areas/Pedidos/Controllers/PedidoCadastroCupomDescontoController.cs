using System;
using System.Web.Mvc;
using BLL.CuponsDesconto;
using BLL.PedidosTemp;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoCadastroCupomDescontoController : Controller {

        //Constantes

        //Atributos
        private ICupomDescontoBL _ICupomDescontoBL;
        private IPedidoTempBL _IPedidoTempBL;

        //Propriedades
        private ICupomDescontoBL OCupomDescontoBL => this._ICupomDescontoBL = this._ICupomDescontoBL ?? new CupomDescontoBL();
        private IPedidoTempBL OPedidoTempBL => _IPedidoTempBL = _IPedidoTempBL ?? new PedidoTempBL();


        //Adicionar um cupom de desconto ao pedido
        // Resposta para função ajax
        [HttpPost, ActionName("adicionar-cupom-desconto")]
        public ActionResult adicionarCupomDesconto() {
            
            string cupomDesconto = UtilRequest.getString("cupomDesconto");

            if (cupomDesconto.isEmpty()) {
                return Json(new { error= true, message = "Informe o cupom de desconto!" }, JsonRequestBehavior.AllowGet);
            }

            var OCupomDesconto = this.OCupomDescontoBL.carregarPorCodigo(cupomDesconto);

            if (OCupomDesconto == null) {
                return Json(new { error= true, message = "Cupom de desconto não encontrado!" }, JsonRequestBehavior.AllowGet);
            }

            if (OCupomDesconto.flagUtilizado) {
                return Json(new { error= true, message = "Este cupom de desconto já foi utilizado!" }, JsonRequestBehavior.AllowGet);
            }

            if (!OCupomDesconto.flagPedido) {
                return Json(new { error= true, message = "Este cupom de desconto não pode ser utilizado em pedidos!" }, JsonRequestBehavior.AllowGet);
            }

            var OPedidoTemp = this.OPedidoTempBL.carregar(Session.SessionID);

            OPedidoTemp.idCupomDesconto = OCupomDesconto.id;
            OPedidoTemp.valorDesconto = OCupomDesconto.valorDesconto;

            this.OPedidoTempBL.salvar(OPedidoTemp);

            return Json(new { error= false, message = "Cupom de desconto adicionado ao pedido" }, JsonRequestBehavior.AllowGet);
        }

        //Adicionar um cupom de desconto ao pedido
        // Resposta para função ajax
        [HttpPost, ActionName("remover-cupom-desconto")]
        public ActionResult removerCupomDesconto() {

            var OPedidoTemp = this.OPedidoTempBL.carregar(Session.SessionID);

            OPedidoTemp.idCupomDesconto = null;
            OPedidoTemp.valorDesconto = 0;

            this.OPedidoTempBL.salvar(OPedidoTemp);

            return Json(new { error = false, message = "OK" }, JsonRequestBehavior.AllowGet);
        }


    }
}