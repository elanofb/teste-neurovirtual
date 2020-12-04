using BLL.Core.Events;

namespace BLL.Financeiro.Events {

	public class OnTituloCadastrado : EventAggregator {

		//Constantes
		private static OnTituloCadastrado _instance;

		//Construtor
		public static OnTituloCadastrado getInstance => _instance ?? (_instance = new OnTituloCadastrado());

	    //Private Construtor para Singleton
		private OnTituloCadastrado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnTituloCadastradoHandler>((source as OnTituloCadastradoHandler));
		}
	}
}