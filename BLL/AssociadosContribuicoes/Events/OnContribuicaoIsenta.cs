using BLL.Core.Events;

namespace BLL.AssociadosContribuicoes.Events {

	public class OnContribuicaoIsenta : EventAggregator {

		//Constantes
		private static OnContribuicaoIsenta _instance;

		//Construtor
		public static OnContribuicaoIsenta getInstance => _instance ?? (_instance = new OnContribuicaoIsenta());

	    //Private Construtor para Singleton
		private OnContribuicaoIsenta() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnContribuicaoIsentaHandler>(source as OnContribuicaoIsentaHandler);
		}
	}
}