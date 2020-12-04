using BLL.Core.Events;

namespace BLL.Financeiro.Events {

	public class OnAtualizarValorTituloDespesa : EventAggregator {

		//Constantes
		private static OnAtualizarValorTituloDespesa _instance;

		//Construtor
		public static OnAtualizarValorTituloDespesa getInstance => _instance = new OnAtualizarValorTituloDespesa();

	    //Private Construtor para Singleton
		private OnAtualizarValorTituloDespesa() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnAtualizarValorTituloDespesaHandler>((source as OnAtualizarValorTituloDespesaHandler));
		}
	}
}