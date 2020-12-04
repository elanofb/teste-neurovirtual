using BLL.Core.Events;

namespace BLL.Associados.Events {

	public class OnNaoAssociadoReativacao : EventAggregator {

		//Constantes
		private static OnNaoAssociadoReativacao _instance;

		//Construtor
		public static OnNaoAssociadoReativacao getInstance => _instance ?? (_instance = new OnNaoAssociadoReativacao());

	    //Private Construtor para Singleton
		private OnNaoAssociadoReativacao() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}
		
		//
		public override void subscribe(object source) {
			this.subscribe<OnNaoAssociadoReativacaoHandler>((source as OnNaoAssociadoReativacaoHandler));
		}
	}
}