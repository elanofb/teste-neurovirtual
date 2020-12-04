using BLL.Core.Events;

namespace BLL.AssociadosInstitucional.Events {

	public class OnAssociadoInstitucionalCadastrado : EventAggregator {

		//Constantes
		private static OnAssociadoInstitucionalCadastrado _instance;

		//Construtor
		public static OnAssociadoInstitucionalCadastrado getInstance {
			get { return _instance ?? (_instance = new OnAssociadoInstitucionalCadastrado()); }
		}

		//Private Construtor para Singleton
		private OnAssociadoInstitucionalCadastrado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnAssociadoInstitucionalCadastradoHandler>((source as OnAssociadoInstitucionalCadastradoHandler));
		}
	}
}