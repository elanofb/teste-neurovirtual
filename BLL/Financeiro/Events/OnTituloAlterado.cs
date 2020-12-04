using BLL.Core.Events;

namespace BLL.Financeiro.Events {

	public class OnTituloAlterado : EventAggregator {

		//Constantes
		private static OnTituloAlterado _instance;

		//Construtor
		public static OnTituloAlterado getInstance => _instance ?? (_instance = new OnTituloAlterado());

	    //Private Construtor para Singleton
		private OnTituloAlterado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnTituloAlteradoHandler>((source as OnTituloAlteradoHandler));
		}
	}
}