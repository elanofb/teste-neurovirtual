using BLL.Core.Events;

namespace BLL.NaoAssociadosInstitucional.Events {

	public class OnSenhaAlterada : EventAggregator {

		//Constantes
		private static OnSenhaAlterada _instance;

		//Construtor
		public static OnSenhaAlterada getInstance {
			get { return _instance ?? (_instance = new OnSenhaAlterada()); }
		}

		//Private Construtor para Singleton
		private OnSenhaAlterada() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnSenhaAlteradaHandler>((source as OnSenhaAlteradaHandler));
		}
	}
}