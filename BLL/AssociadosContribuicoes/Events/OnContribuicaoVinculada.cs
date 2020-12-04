using BLL.Core.Events;

namespace BLL.AssociadosContribuicoes.Events {

	public class OnContribuicaoVinculada : EventAggregator {

		//Constantes
		private static OnContribuicaoVinculada _instance;

		//Construtor
		public static OnContribuicaoVinculada getInstance {
			get { return _instance ?? (_instance = new OnContribuicaoVinculada()); }
		}

		//Private Construtor para Singleton
		private OnContribuicaoVinculada() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			//this.subscribe<OnContribuicaoVinculadaHandler>((source as OnContribuicaoVinculadaHandler));
		}
	}
}