using BLL.Core.Events;

namespace BLL.Financeiro.Events {

	public class OnTituloQuitado : EventAggregator {

		//Constantes
		private static OnTituloQuitado _instance;

		//Construtor
		public static OnTituloQuitado getInstance {
			get { return _instance ?? (_instance = new OnTituloQuitado()); }
		}

		//Private Construtor para Singleton
		public OnTituloQuitado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnTituloQuitadoHandler>((source as OnTituloQuitadoHandler));
		}
	}
}