using BLL.Core.Events;

namespace BLL.Planos {

	public class OnPlanoContratado : EventAggregator {

		//Constantes
		private static OnPlanoContratado _instance;

		//Construtor
		public static OnPlanoContratado getInstance {
			get { return _instance ?? (_instance = new OnPlanoContratado()); }
		}

		//Private Construtor para Singleton
		private OnPlanoContratado() {
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
			this.subscribe<OnPlanoContratadoHandler>((source as OnPlanoContratadoHandler));
		}
	}
}