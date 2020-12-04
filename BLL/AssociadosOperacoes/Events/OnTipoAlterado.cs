using BLL.Core.Events;

namespace BLL.AssociadosOperacoes.Events {

	public class OnTipoAlterado : EventAggregator {

		//Constantes
		private static OnTipoAlterado _instance;

		//Construtor
		public static OnTipoAlterado getInstance => _instance = _instance ?? new OnTipoAlterado();

		//Private Construtor para Singleton
		private OnTipoAlterado() {

		}

		public override void publish(object source) {
			this.publish<object>(source);
		}

		public override void subscribe(object source) {
			this.subscribe((source as OnTipoAlteradoHandler));
		}

	}
}