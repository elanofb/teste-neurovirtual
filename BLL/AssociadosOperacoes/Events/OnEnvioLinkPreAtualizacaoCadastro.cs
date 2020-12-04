using BLL.Core.Events;

namespace BLL.AssociadosOperacoes {

	public class OnEnvioLinkPreAtualizacaoCadastro : EventAggregator {

		//Constantes
		private static OnEnvioLinkPreAtualizacaoCadastro _instance;

		//Construtor
		public static OnEnvioLinkPreAtualizacaoCadastro getInstance => _instance = _instance ?? new OnEnvioLinkPreAtualizacaoCadastro();

		//Private Construtor para Singleton
		private OnEnvioLinkPreAtualizacaoCadastro() {
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
			this.subscribe((source as OnEnvioLinkPreAtualizacaoCadastroHandler));
		}
	}
}