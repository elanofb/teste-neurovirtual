using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesEcommerce;
using BLL.Pedidos;
using DAL.Enderecos;
using DAL.Pedidos;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoDetalhesEnderecoEntregaController : Controller {

        //Constantes

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoEntregaOperacaoBL _IPedidoEntregaOperacaoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();
        private IPedidoEntregaOperacaoBL OPedidoEntregaOperacaoBL => _IPedidoEntregaOperacaoBL = _IPedidoEntregaOperacaoBL ?? new PedidoEntregaOperacaoBL();

        //
        [HttpGet, ActionName("modal-alterar-endereco-entrega")]
        public ActionResult modalAlterarEnderecoEntrega(int? id) {
            
            var OPedido = this.OPedidoBL.carregar(id.toInt());

            if (OPedido == null) {
                return Json(new { flagError = true, message = "O pedido informado não foi encontrado." });
            }

            var ViewModel = new PedidoDetalhesFreteForm();
            
            ViewModel.PedidoEntrega = OPedido.listaPedidoEntrega.FirstOrDefault(x => x.flagExcluido == "N");

            if (ViewModel.PedidoEntrega == null) {
                
                ViewModel.PedidoEntrega = new PedidoEntrega() {
                    idPedido = id.toInt(),
                    cepOrigem = ConfiguracaoEcommerceBL.getInstance.carregar().cepOrigemFrete
                };

            }

            return View(ViewModel);

        }

        //
        [HttpPost]
        public ActionResult salvar(PedidoDetalhesFreteForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-alterar-endereco-entrega", ViewModel);
            }

            ViewModel.PedidoEntrega.idPais = "BRA";

            ViewModel.PedidoEntrega.idTipoEndereco = TipoEnderecoConst.PRINCIPAL;

            var flagSucesso = this.OPedidoEntregaOperacaoBL.salvar(ViewModel.PedidoEntrega);

            if (flagSucesso) {

                return Json(new { error = false }, JsonRequestBehavior.AllowGet);    

            }

            return Json(new { error = true, message = "Houve algum problema ao alterar o endereço de entrega do pedido." }, JsonRequestBehavior.AllowGet);

        }
        
    }

}