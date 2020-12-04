using BLL.Core.Events;

namespace BLL.Financeiro.Events {

	public class OnPagamentoRecusado : EventAggregator {

		//Constantes
		private static OnPagamentoRecusado _instance;

		//Construtor
		public static OnPagamentoRecusado getInstance => _instance ?? (_instance = new OnPagamentoRecusado());

	    //Private Construtor para Singleton
		private OnPagamentoRecusado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnPagamentoRecusadoHandler>((source as OnPagamentoRecusadoHandler));
		}
	}
}