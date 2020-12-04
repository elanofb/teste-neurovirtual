using BLL.Core.Events;

namespace BLL.Atendimentos {

	public class OnHistoricoCadastrado : EventAggregator {

		//Constantes
		private static OnHistoricoCadastrado _instance;

		//Construtor
		public static OnHistoricoCadastrado getInstance => _instance ?? (_instance = new OnHistoricoCadastrado());

        //Private Construtor para Singleton
	    public OnHistoricoCadastrado() {
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
			this.subscribe((source as OnHistoricoCadastradoHandler));
		}
	}
}