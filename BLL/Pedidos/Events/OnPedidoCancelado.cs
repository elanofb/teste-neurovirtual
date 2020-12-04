using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnPedidoCancelado : EventAggregator {

		//Constantes
		private static OnPedidoCancelado _instance;

		//Construtor
		public static OnPedidoCancelado getInstance {
			get { return _instance ?? (_instance = new OnPedidoCancelado()); }
		}

		//Private Construtor para Singleton
		private OnPedidoCancelado() {
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
			this.subscribe((source as OnPedidoCanceladoHandler));
		}
	}
}