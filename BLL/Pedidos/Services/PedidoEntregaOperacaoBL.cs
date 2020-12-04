using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Core.Events;
using BLL.Frete;
using BLL.PedidosTemp;
using BLL.Pessoas;
using BLL.Services;
using DAL.Pedidos;
using DAL.PedidosTemp;
using DAL.Pessoas;

namespace BLL.Pedidos {

    public class PedidoEntregaOperacaoBL : DefaultBL, IPedidoEntregaOperacaoBL {
        
		//Atributos
        private IPedidoEntregaBL _IPedidoEntregaBL;

		//Propriedades
        private IPedidoEntregaBL OPedidoEntregaBL => _IPedidoEntregaBL = _IPedidoEntregaBL ?? new PedidoEntregaBL();
        
        // Events
        private EventAggregator onEnderecoEntregaAlterado => OnEnderecoEntregaAlterado.getInstance;

		//
		public PedidoEntregaOperacaoBL() {
            
        }
        
        //
        public bool salvar(PedidoEntrega OPedidoEntrega) {
            
            var flagSucesso = this.OPedidoEntregaBL.salvar(OPedidoEntrega);

            if (flagSucesso) {

                this.onEnderecoEntregaAlterado.subscribe(new OnEnderecoEntregaAlteradoHandler());

                this.onEnderecoEntregaAlterado.publish(OPedidoEntrega.idPedido as object);

            }

            return flagSucesso;

        }

	}

}