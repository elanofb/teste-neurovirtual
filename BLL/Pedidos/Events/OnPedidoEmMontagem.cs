using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnPedidoEmMontagem : EventAggregator {

		//Constantes
		private static OnPedidoEmMontagem _instance;

		//Construtor
		public static OnPedidoEmMontagem getInstance => _instance = _instance ?? new OnPedidoEmMontagem();
        
		//Private Construtor para Singleton
		private OnPedidoEmMontagem() {
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
			this.subscribe((source as OnPedidoEmMontagemHandler));
		}
	}
}