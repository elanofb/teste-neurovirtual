using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnPedidoPreparadoExpedicao : EventAggregator {

		//Constantes
		private static OnPedidoPreparadoExpedicao _instance;

		//Construtor
		public static OnPedidoPreparadoExpedicao getInstance => _instance = _instance ?? new OnPedidoPreparadoExpedicao();
        
		//Private Construtor para Singleton
		private OnPedidoPreparadoExpedicao() {
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
			this.subscribe((source as OnPedidoPreparadoExpedicaoHandler));
		}
	}
}