using BLL.Core.Events;

namespace BLL.AssociadosOperacoes.Events {

	public class OnAssociadoAtivado : EventAggregator {

		//Constantes
		private static OnAssociadoAtivado _instance;

		//Construtor
		public static OnAssociadoAtivado getInstance {
			get { return _instance ?? (_instance = new OnAssociadoAtivado()); }
		}

		//Private Construtor para Singleton
		private OnAssociadoAtivado() {
		}

		public override void publish(object source) {
			this.publish<object>(source);
		}

		public override void subscribe(object source) {
			this.subscribe<OnAssociadoAtivadoHandler>((source as OnAssociadoAtivadoHandler));
		}
	}
}