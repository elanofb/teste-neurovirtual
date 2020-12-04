using BLL.Core.Events;

namespace BLL.Atendimentos {

	public class OnAtendimentoIniciado : EventAggregator {

		//Constantes
		private static OnAtendimentoIniciado _instance;

		//Construtor
		public static OnAtendimentoIniciado getInstance => _instance ?? (_instance = new OnAtendimentoIniciado());

        //Private Construtor para Singleton
	    public OnAtendimentoIniciado() {
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
			this.subscribe((source as OnAtendimentoIniciadoHandler));
		}
	}
}