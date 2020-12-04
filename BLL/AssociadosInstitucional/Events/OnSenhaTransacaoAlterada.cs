using BLL.Core.Events;

namespace BLL.AssociadosInstitucional.Events {

	public class OnSenhaTransacaoAlterada : EventAggregator {

		//Constantes
		private static OnSenhaTransacaoAlterada _instance;

		//Construtor
		public static OnSenhaTransacaoAlterada getInstance {
			get { return _instance ?? (_instance = new OnSenhaTransacaoAlterada()); }
		}

		//Private Construtor para Singleton
		private OnSenhaTransacaoAlterada() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnSenhaTransacaoAlteradaHandler>((source as OnSenhaTransacaoAlteradaHandler));
		}
	}
}