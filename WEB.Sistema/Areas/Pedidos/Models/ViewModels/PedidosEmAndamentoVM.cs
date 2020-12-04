using System.Collections.Generic;
using System.Linq;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels{

	public class PedidosEmAndamentoVM {

	    public List<Pedido> listaPedidos { get; set; }

        public int qtdeAguardandoPagamento { get; set; }

        public int qtdePago { get; set; }

        public int qtdeAtendido { get; set; }

    	//
		public PedidosEmAndamentoVM() { 
			
            this.listaPedidos = new List<Pedido>();
		}

        //carregarTotais
        public void carregarTotais() {

            this.qtdeAguardandoPagamento = this.listaPedidos.Count(x => x.idStatusPedido == StatusPedidoConst.AGUARDANDO_PAGAMENTO);

            this.qtdeAtendido = this.listaPedidos.Count(x => x.idStatusPedido == StatusPedidoConst.ATENDIDO);

            this.qtdePago = this.listaPedidos.Count(x => x.idStatusPedido == StatusPedidoConst.PAGO);
        }

	}

}