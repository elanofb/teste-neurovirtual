using BLL.Core.Events;

namespace BLL.Pessoas {

	public class OnPessoaAlterada : EventAggregator {

		//Constantes
		private static OnPessoaAlterada _instance;

		//Construtor
		public static OnPessoaAlterada getInstance => _instance = _instance ?? new OnPessoaAlterada();
        
		//Private Construtor para Singleton
		private OnPessoaAlterada() {
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
			this.subscribe((source as OnPessoaAlteradaHandler));
		}
	}
}