using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnProdutoRemovido : EventAggregator {

		//Constantes
		private static OnProdutoRemovido _instance;

		//Construtor
		public static OnProdutoRemovido getInstance => _instance = _instance ?? new OnProdutoRemovido();

		//Private Construtor para Singleton
		private OnProdutoRemovido() {
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
			this.subscribe((source as OnProdutoRemovidoHandler));
		}
	}
}