using BLL.Core.Events;

namespace BLL.Notificacoes.Events {

	public class OnNotificacaoExcluida : EventAggregator {

		//Constantes
		private static OnNotificacaoExcluida _instance;

		//Construtor
		public static OnNotificacaoExcluida getInstance {
			get { return _instance ?? (_instance = new OnNotificacaoExcluida()); }
		}

		//Private Construtor para Singleton
		private OnNotificacaoExcluida() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnNotificacaoExcluidaHandler>((source as OnNotificacaoExcluidaHandler));
		}
	}
}