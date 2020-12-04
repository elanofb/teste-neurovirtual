using BLL.Core.Events;

namespace BLL.Pedidos {

	public class OnFretePedidoEscolhido : EventAggregator {

		//Constantes
		private static OnFretePedidoEscolhido _instance;

		//Construtor
		public static OnFretePedidoEscolhido getInstance {
			get { return _instance ?? (_instance = new OnFretePedidoEscolhido()); }
		}

		//Private Construtor para Singleton
		private OnFretePedidoEscolhido() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

        //
		public override void subscribe(object source) {
			this.subscribe<FretePedidoEscolhidoHandler>((source as FretePedidoEscolhidoHandler));
		}
	}
}