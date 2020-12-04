using BLL.Core.Events;

namespace BLL.Associados {

	public class OnAssociadoAlterado : EventAggregator {

		//Constantes
		private static OnAssociadoAlterado _instance;

		//Construtor
		public static OnAssociadoAlterado getInstance => _instance ?? (_instance = new OnAssociadoAlterado());

	    //Private Construtor para Singleton
		private OnAssociadoAlterado() {
		}

		/**
		*
		*/

		public override void publish(object source) {
			this.publish<object>(source);
		}

		/**
		*
		*/

		public override void subscribe(object source) {
			this.subscribe<OnAssociadoAlteradoHandler>((source as OnAssociadoAlteradoHandler));
		}
	}
}