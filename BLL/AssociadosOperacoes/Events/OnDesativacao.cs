using BLL.Core.Events;

namespace BLL.AssociadosOperacoes.Events {

	public class OnDesativacao : EventAggregator {

		//Constantes
		private static OnDesativacao _instance;

		//Construtor
		public static OnDesativacao getInstance {
			get { return _instance ?? (_instance = new OnDesativacao()); }
		}

		//Private Construtor para Singleton
		private OnDesativacao() {
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
			this.subscribe<OnDesativacaoHandler>((source as OnDesativacaoHandler));
		}
	}
}