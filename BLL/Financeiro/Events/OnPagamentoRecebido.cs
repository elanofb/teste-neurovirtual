using BLL.Core.Events;

namespace BLL.Financeiro.Events {

	public class OnPagamentoRecebido : EventAggregator {

		//Constantes
		private static OnPagamentoRecebido _instance;

		//Construtor
		public static OnPagamentoRecebido getInstance => _instance ?? (_instance = new OnPagamentoRecebido());

	    //Private Construtor para Singleton
		public OnPagamentoRecebido() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe((source as OnPagamentoRecebidoHandler));
		}
	}
}