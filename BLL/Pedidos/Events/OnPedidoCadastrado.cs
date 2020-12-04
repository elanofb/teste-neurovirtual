using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnPedidoCadastrado : EventAggregator {

		//Constantes
		private static OnPedidoCadastrado _instance;

		//Construtor
		public static OnPedidoCadastrado getInstance {
			get { return _instance ?? (_instance = new OnPedidoCadastrado()); }
		}

		//Private Construtor para Singleton
		private OnPedidoCadastrado() {
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
			this.subscribe<PedidoCadastradoHandler>((source as PedidoCadastradoHandler));
		}
	}
}