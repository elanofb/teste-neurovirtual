using BLL.Core.Events;

namespace BLL.FinanceiroLancamentos.Events {

	public class OnReceitaCadastrada : EventAggregator {

		//Constantes
		private static OnReceitaCadastrada _instance;

		//Construtor
		public static OnReceitaCadastrada getInstance => _instance ?? (_instance = new OnReceitaCadastrada());

	    //Private Construtor para Singleton
		private OnReceitaCadastrada() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnReceitaCadastradaHandler>((source as OnReceitaCadastradaHandler));
		}
	}
}