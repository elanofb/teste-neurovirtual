using BLL.Core.Events;

namespace BLL.Atendimentos {

	public class OnHistoricoMensagemCadastrado : EventAggregator {

		//Constantes
		private static OnHistoricoMensagemCadastrado _instance;

		//Construtor
		public static OnHistoricoMensagemCadastrado getInstance => _instance ?? (_instance = new OnHistoricoMensagemCadastrado());

        //Private Construtor para Singleton
	    public OnHistoricoMensagemCadastrado() {
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
			this.subscribe((source as OnHistoricoMensagemCadastradoHandler));
		}
	}
}