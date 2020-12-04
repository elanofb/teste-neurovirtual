using BLL.Core.Events;
using BLL.NaoAssociados.Events;

namespace BLL.NaoAssociados {

	public class OnNaoAssociadoCadastrado : EventAggregator {

		//Constantes
		private static OnNaoAssociadoCadastrado _instance;

		//Construtor
		public static OnNaoAssociadoCadastrado getInstance => _instance ?? (_instance = new OnNaoAssociadoCadastrado());

	    //Private Construtor para Singleton
		private OnNaoAssociadoCadastrado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnNaoAssociadoCadastradoHandler>((source as OnNaoAssociadoCadastradoHandler));
		}
	}
}