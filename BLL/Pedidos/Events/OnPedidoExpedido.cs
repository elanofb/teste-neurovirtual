using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnPedidoExpedido : EventAggregator {

		//Constantes
		private static OnPedidoExpedido _instance;

		//Construtor
		public static OnPedidoExpedido getInstance => _instance = _instance ?? new OnPedidoExpedido();
        
		//Private Construtor para Singleton
		private OnPedidoExpedido() {
		}

		/**
		*
		*/

		public override void publish(object source) {
			this.publish<object>(source);
		}

		/**
		*
		*/

		public override void subscribe(object source) {
			this.subscribe((source as OnPedidoExpedidoHandler));
		}
	}
}