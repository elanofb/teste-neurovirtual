using BLL.Core.Events;

namespace BLL.AssociadosCarteirinha.Events {

	public class OnEnvioCarteirinha : EventAggregator {

		//Constantes
		private static OnEnvioCarteirinha _instance;

		//Construtor
		public static OnEnvioCarteirinha getInstance {
			get { return _instance ?? (_instance = new OnEnvioCarteirinha()); }
		}

		//Private Construtor para Singleton
		private OnEnvioCarteirinha() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnEnvioCarteirinhaHandler>((source as OnEnvioCarteirinhaHandler));
		}
	}
}