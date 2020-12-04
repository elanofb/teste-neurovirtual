using BLL.Core.Events;

namespace BLL.Associados.Events {

	public class OnAdmissao : EventAggregator {

		//Constantes
		private static OnAdmissao _instance;

		//Construtor
		public static OnAdmissao getInstance {
			get { return _instance ?? (_instance = new OnAdmissao()); }
		}

		//Private Construtor para Singleton
		private OnAdmissao() {
		}

		public override void publish(object source) {
			this.publish<object>(source);
		}

		public override void subscribe(object source) {
			this.subscribe<OnAdmissaoHandler>((source as OnAdmissaoHandler));
		}
	}
}