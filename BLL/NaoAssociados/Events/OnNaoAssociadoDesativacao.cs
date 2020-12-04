using BLL.Core.Events;

namespace BLL.NaoAssociados.Events {

	public class OnNaoAssociadoDesativacao : EventAggregator {

		//Constantes
		private static OnNaoAssociadoDesativacao _instance;

		//Construtor
		public static OnNaoAssociadoDesativacao getInstance => _instance ?? (_instance = new OnNaoAssociadoDesativacao());

	    //Private Construtor para Singleton
		private OnNaoAssociadoDesativacao() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnNaoAssociadoDesativacaoHandler>((source as OnNaoAssociadoDesativacaoHandler));
		}
	}
}