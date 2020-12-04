using BLL.Core.Events;
using BLL.NaoAssociados.Events;

namespace BLL.NaoAssociados {

	public class OnAreaAssociadoNaoAssociadoCadastrado : EventAggregator {

		//Constantes
		private static OnAreaAssociadoNaoAssociadoCadastrado _instance;

		//Construtor
		public static OnAreaAssociadoNaoAssociadoCadastrado getInstance => _instance ?? (_instance = new OnAreaAssociadoNaoAssociadoCadastrado());

	    //Private Construtor para Singleton
		private OnAreaAssociadoNaoAssociadoCadastrado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnAreaAssociadoNaoAssociadoCadastradoHandler>((source as OnAreaAssociadoNaoAssociadoCadastradoHandler));
		}
	}
}