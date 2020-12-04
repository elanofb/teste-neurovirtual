using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnProdutoAdicionado : EventAggregator {

		//Constantes
		private static OnProdutoAdicionado _instance;

		//Construtor
		public static OnProdutoAdicionado getInstance => _instance = _instance ?? new OnProdutoAdicionado();

		//Private Construtor para Singleton
		private OnProdutoAdicionado() {
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
			this.subscribe((source as OnProdutoAdicionadoHandler));
		}
	}
}