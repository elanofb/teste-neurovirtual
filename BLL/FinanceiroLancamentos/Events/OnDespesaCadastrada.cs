using BLL.Core.Events;

namespace BLL.FinanceiroLancamentos.Events {

	public class OnDespesaCadastrada : EventAggregator {

		//Constantes
		private static OnDespesaCadastrada _instance;

		//Construtor
		public static OnDespesaCadastrada getInstance => _instance ?? (_instance = new OnDespesaCadastrada());

	    //Private Construtor para Singleton
		private OnDespesaCadastrada() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnDespesaCadastradaHandler>((source as OnDespesaCadastradaHandler));
		}
	}
}