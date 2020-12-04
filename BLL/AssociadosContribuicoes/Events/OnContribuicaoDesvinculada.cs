using BLL.Core.Events;

namespace BLL.AssociadosContribuicoes.Events {

	public class OnContribuicaoDesvinculada : EventAggregator {

		//Constantes
		private static OnContribuicaoDesvinculada _instance;

		//Construtor
		public static OnContribuicaoDesvinculada getInstance {
			get { return _instance ?? (_instance = new OnContribuicaoDesvinculada()); }
		}

		//Private Construtor para Singleton
		private OnContribuicaoDesvinculada() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe(source as OnContribuicaoDesvinculadaHandler);
		}
	}
}