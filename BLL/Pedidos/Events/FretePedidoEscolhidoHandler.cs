using BLL.Core.Events;
using System;
using BLL.Checkout;
using BLL.Financeiro;
using DAL.Checkout;
using System.Linq;

namespace BLL.Pedidos {

	public class FretePedidoEscolhidoHandler : IHandler<object> {

        //Atributos

        //Propriedades
        private IPedidoBL OPedidoBL => new PedidoBL();

        //
        public void execute(object source) {

            var idCheckoutCompra = (int)source;

            var OCheckoutCompra = OCheckoutCompraBL.carregar(idCheckoutCompra);

            //Vincula o frete no pedido
            var OCheckoutEntrega = OCheckoutCompra.listaEntregas.FirstOrDefault(x => x.dtExclusao == null) ?? new CheckoutEntrega();

            OPedidoBL.salvarFrete(
                Convert.ToInt32(OCheckoutCompra.TituloReceita.idReceita),
                Convert.ToDecimal(OCheckoutCompra.valorFrete),
                Convert.ToInt32(OCheckoutEntrega.idTipoFrete)
                //Convert.ToInt32(OCheckoutEntrega.prazoEntrega)
            );

            //Atualiza o valor total
            //var OTituloReceita = OTituloReceitaBL.carregar(OCheckoutCompra.idTituloReceita);
            //OTituloReceita.valorTotal = OCheckoutCompra.valorTotalCompra();
            //OTituloReceitaBL.salvar(OTituloReceita);
        }
	}
}