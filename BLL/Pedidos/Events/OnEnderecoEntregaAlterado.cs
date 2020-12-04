using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnEnderecoEntregaAlterado : EventAggregator {

		//Constantes
		private static OnEnderecoEntregaAlterado _instance;

		//Construtor
		public static OnEnderecoEntregaAlterado getInstance => _instance = _instance ?? new OnEnderecoEntregaAlterado();

		//Private Construtor para Singleton
		private OnEnderecoEntregaAlterado() {
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
			this.subscribe((source as OnEnderecoEntregaAlteradoHandler));
		}
	}
}