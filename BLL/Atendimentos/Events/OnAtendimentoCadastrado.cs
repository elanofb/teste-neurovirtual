using BLL.Core.Events;

namespace BLL.Atendimentos {

	public class OnAtendimentoCadastrado : EventAggregator {

		//Constantes
		private static OnAtendimentoCadastrado _instance;

		//Construtor
		public static OnAtendimentoCadastrado getInstance => _instance ?? (_instance = new OnAtendimentoCadastrado());

	    //Private Construtor para Singleton
		private OnAtendimentoCadastrado() {
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
			this.subscribe((source as OnAtendimentoCadastradoHandler));
		}
	}
}