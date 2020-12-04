using BLL.Core.Events;

namespace BLL.Notificacoes.Events {

	public class OnNotificacaoCadastrada : EventAggregator {

		//Constantes
		private static OnNotificacaoCadastrada _instance;

		//Construtor
		public static OnNotificacaoCadastrada getInstance {
			get { return _instance ?? (_instance = new OnNotificacaoCadastrada()); }
		}

		//Private Construtor para Singleton
		private OnNotificacaoCadastrada() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnNotificacaoCadastradaHandler>((source as OnNotificacaoCadastradaHandler));
		}
	}
}