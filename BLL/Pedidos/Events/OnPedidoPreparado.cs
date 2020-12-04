using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnPedidoPreparado : EventAggregator {

		//Constantes
		private static OnPedidoPreparado _instance;

		//Construtor
		public static OnPedidoPreparado getInstance => _instance = _instance ?? new OnPedidoPreparado();
        
		//Private Construtor para Singleton
		private OnPedidoPreparado() {
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
			this.subscribe((source as OnPedidoPreparadoHandler));
		}
	}
}