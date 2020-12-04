using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnPedidoAtendido : EventAggregator {

		//Constantes
		private static OnPedidoAtendido _instance;

		//Construtor
		public static OnPedidoAtendido getInstance => _instance = _instance ?? new OnPedidoAtendido();
        
		//Private Construtor para Singleton
		private OnPedidoAtendido() {
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
			this.subscribe((source as OnPedidoAtendidoHandler));
		}
	}
}