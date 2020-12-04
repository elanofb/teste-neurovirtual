using BLL.Core.Events;

namespace BLL.ContasBancarias {

	public class OnTransferenciaRealizada : EventAggregator {

		//Constantes
		private static OnTransferenciaRealizada _instance;

		//Construtor
		public static OnTransferenciaRealizada getInstance => _instance = _instance ?? new OnTransferenciaRealizada();

		//Private Construtor para Singleton
		private OnTransferenciaRealizada() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe((source as OnTransferenciaRealizadaHandler));
		}

	}

}