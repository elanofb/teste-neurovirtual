using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnPedidoFinalizado : EventAggregator {

		//Constantes
		private static OnPedidoFinalizado _instance;

		//Construtor
		public static OnPedidoFinalizado getInstance => _instance = _instance ?? new OnPedidoFinalizado();
        
		//Private Construtor para Singleton
		private OnPedidoFinalizado() {
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
			this.subscribe((source as OnPedidoFinalizadoHandler));
		}
	}
}