using BLL.Core.Events;

namespace BLL.Financeiro.Events {

	public class OnAtualizarValorTituloReceita : EventAggregator {

		//Constantes
		private static OnAtualizarValorTituloReceita _instance;

		//Construtor
		public static OnAtualizarValorTituloReceita getInstance => _instance = new OnAtualizarValorTituloReceita();

	    //Private Construtor para Singleton
		private OnAtualizarValorTituloReceita() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnAtualizarValorTituloReceitaHandler>((source as OnAtualizarValorTituloReceitaHandler));
		}
	}
}