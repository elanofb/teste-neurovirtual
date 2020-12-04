using BLL.Core.Events;

namespace BLL.AssociadosOperacoes.Events {

	public class OnAdmissaoAlterada : EventAggregator {

		//Constantes
		private static OnAdmissaoAlterada _instance;

		//Construtor
		public static OnAdmissaoAlterada getInstance => _instance = _instance ?? new OnAdmissaoAlterada();

		//Private Construtor para Singleton
		private OnAdmissaoAlterada() {

		}

		public override void publish(object source) {
			this.publish<object>(source);
		}

		public override void subscribe(object source) {
			this.subscribe((source as OnAdmissaoAlteradaHandler));
		}
	}
}