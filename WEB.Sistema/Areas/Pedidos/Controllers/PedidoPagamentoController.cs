using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using BLL.Pedidos;
using DAL.Financeiro;
using DAL.Pedidos;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoPagamentoController : Controller {

        //Atributos
        private IPedidoBL _IPedidoBL;
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
        private IPedidoBL OPedidoBL => this._IPedidoBL = this._IPedidoBL ?? new PedidoBL();
        private ITituloReceitaBL OTituloReceitaBL => this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaPedidoBL();

        [ActionName("partial-dados-pagamento")]
        public ActionResult partialDadosPagamento(int idPedido) {
            
            var ViewModel = new PedidoPagamentoVM();

            ViewModel.Pedido = this.OPedidoBL.carregar(idPedido);

            var OTituloPedido = this.OTituloReceitaBL.carregarPorReceita(ViewModel.Pedido.id) ?? new TituloReceita();

            ViewModel.listaPagamentos = OTituloPedido.listaTituloReceitaPagamento.Where(x => x.dtExclusao == null).ToList();

            ViewModel.flagTemPagamento = ViewModel.listaPagamentos.Any();

            if (OTituloPedido.id == 0 && ViewModel.Pedido.idStatusPedido != StatusPedidoConst.CANCELADO) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Atenção! Ocorreram problemas para gerar esse pedido, por favor, cancele este e crie um novo");
            }

            return PartialView(ViewModel);
        }

        [ActionName("partial-carregar-parcelas")]
        public ActionResult partialCarregarParcelas(int idPedido, int? qtdeParcelas) {

            if (UtilNumber.toInt32(qtdeParcelas) == 0) {
                return Json( new {error = true, message="Informe uma quantidade de parcelas maior do que zero."});
            }

            var ViewModel = new PedidoPagamentoVM();

            ViewModel.Pedido = this.OPedidoBL.carregar(idPedido);

            decimal valorParcela = Decimal.Divide(ViewModel.Pedido.getValorTotal(), UtilNumber.toInt32(qtdeParcelas));

            decimal valorTotalParcelas = 0;

            decimal valorTotalPedido = ViewModel.Pedido.getValorTotal();

            for (int i = 0; i < qtdeParcelas; i++) {

                var OParcela = new TituloReceitaPagamento();

                OParcela.valorOriginal = Math.Round(valorParcela, 2);

                OParcela.dtVencimento = DateTime.Today.AddDays(1).AddMonths(i);

                ViewModel.listaPagamentos.Add(OParcela);

                valorTotalParcelas = Math.Round(decimal.Add(valorTotalParcelas, valorParcela), 2);
            }

            decimal valorDiferenca = Decimal.Subtract(valorTotalPedido, valorTotalParcelas);

            var OParcelaUltima = ViewModel.listaPagamentos.LastOrDefault();

            OParcelaUltima.valorOriginal = Decimal.Add(OParcelaUltima.valorOriginal, valorDiferenca);

            ViewModel.flagTemPagamento = false;

            return PartialView("partial-dados-pagamento", ViewModel);
        }

        //
        [HttpPost, ActionName("salvar-parcelas")]
        public ActionResult salvarParcelas(PedidoPagamentoVM ViewModel) {

            string mensagemRetorno = "";

            ViewModel.Pedido = this.OPedidoBL.carregar(ViewModel.Pedido.id);

            if (!ModelState.IsValid) {

                return PartialView("partial-dados-pagamento", ViewModel);

            }

            if (!ViewModel.listaPagamentos.Any()) {

                mensagemRetorno = "Atenção! Selecione a quantidade de parcelas e clique em 'Parcelar' para definir as datas de vencimento.";

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, mensagemRetorno);

                return Json(new {error = false, message = mensagemRetorno});

            }

            if (ViewModel.listaPagamentos.Any(x => x.dtVencimento <= DateTime.Today)) {

                mensagemRetorno = "Atenção! As parcelas devem ter o vencimento superior a hoje.";

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, mensagemRetorno);

                return Json(new {error = false, message = mensagemRetorno});
            }

            var OTituloPedido = this.OTituloReceitaBL.carregarPorReceita(ViewModel.Pedido.id) ?? new TituloReceita();

            if (OTituloPedido.id == 0) {

                mensagemRetorno = "Atenção! Ocorreram problemas para gerar esse pedido, por favor, cancele este e crie um novo.";

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, mensagemRetorno);

                return Json(new {error = false, message = mensagemRetorno});
            }

            ViewModel.listaPagamentos.ForEach(x => {

                x.idUsuarioCadastro = User.id();

                x.idUsuarioAlteracao = User.id();

            });

            //this.OTituloReceitaBL.salvarParcelas(OTituloPedido, ViewModel.listaPagamentos, true);

            mensagemRetorno = "Os dados do parcelamento foram salvos com sucesso.";

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, mensagemRetorno);

            return Json(new {error = false, message = mensagemRetorno});
        }
    }
}