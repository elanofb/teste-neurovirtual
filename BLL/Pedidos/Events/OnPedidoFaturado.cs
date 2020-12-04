using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnPedidoFaturado : EventAggregator {

		//Constantes
		private static OnPedidoFaturado _instance;

		//Construtor
		public static OnPedidoFaturado getInstance {
			get { return _instance ?? (_instance = new OnPedidoFaturado()); }
		}

		//Private Construtor para Singleton
		private OnPedidoFaturado() {
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
			this.subscribe<OnPedidoFaturadoHandler>((source as OnPedidoFaturadoHandler));
		}
	}
}