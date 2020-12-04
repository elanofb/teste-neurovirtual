using BLL.Core.Events;

namespace BLL.AssociadosDependentes.Events {

	public class OnAssociadoDependenteCadastrado : EventAggregator {

		//Constantes
		private static OnAssociadoDependenteCadastrado _instance;

		//Construtor
		public static OnAssociadoDependenteCadastrado getInstance {
			get { return _instance ?? (_instance = new OnAssociadoDependenteCadastrado()); }
		}

		//Private Construtor para Singleton
		private OnAssociadoDependenteCadastrado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnAssociadoDependenteCadastradoHandler>((source as OnAssociadoDependenteCadastradoHandler));
		}
	}
}