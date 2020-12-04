using BLL.Core.Events;

namespace BLL.AssociadosOperacoes.Events {

	public class OnAssociadoExcluido : EventAggregator {

		//Constantes
		private static OnAssociadoExcluido _instance;

		//Construtor
		public static OnAssociadoExcluido getInstance {
			get { return _instance ?? (_instance = new OnAssociadoExcluido()); }
		}

		//Private Construtor para Singleton
		private OnAssociadoExcluido() {
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
			this.subscribe<AssociadoExcluidoHandler>((source as AssociadoExcluidoHandler));
		}
	}
}