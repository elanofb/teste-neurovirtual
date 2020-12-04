using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnPedidoQuitado : EventAggregator {

		//Constantes
		private static OnPedidoQuitado _instance;

		//Construtor
		public static OnPedidoQuitado getInstance {
			get { return _instance ?? (_instance = new OnPedidoQuitado()); }
		}

		//Private Construtor para Singleton
		private OnPedidoQuitado() {
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
			this.subscribe<OnPedidoQuitadoHandler>((source as OnPedidoQuitadoHandler));
		}
	}
}