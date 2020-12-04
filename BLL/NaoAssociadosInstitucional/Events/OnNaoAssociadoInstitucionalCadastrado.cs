using BLL.Core.Events;

namespace BLL.NaoAssociadosInstitucional.Events {

	public class OnNaoAssociadoInstitucionalCadastrado : EventAggregator {

		//Constantes
		private static OnNaoAssociadoInstitucionalCadastrado _instance;

		//Construtor
		public static OnNaoAssociadoInstitucionalCadastrado getInstance => _instance ?? (_instance = new OnNaoAssociadoInstitucionalCadastrado());

	    //Private Construtor para Singleton
		private OnNaoAssociadoInstitucionalCadastrado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnNaoAssociadoInstitucionalCadastradoHandler>((source as OnNaoAssociadoInstitucionalCadastradoHandler));
		}
	}
}