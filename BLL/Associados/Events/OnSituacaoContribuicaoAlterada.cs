using BLL.Core.Events;

namespace BLL.Associados.Events {

	public class OnSituacaoContribuicaoAlterada : EventAggregator {

		//Constantes
		private static OnSituacaoContribuicaoAlterada _instance;

		//Construtor
		public static OnSituacaoContribuicaoAlterada getInstance {
			get { return _instance ?? (_instance = new OnSituacaoContribuicaoAlterada()); }
		}

		//Private Construtor para Singleton
		public OnSituacaoContribuicaoAlterada() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe((source as OnSituacaoContribuicaoAlteradaHandler));
		}
	}
}