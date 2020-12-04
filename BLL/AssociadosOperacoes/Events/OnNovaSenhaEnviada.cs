using BLL.Core.Events;

namespace BLL.AssociadosOperacoes.Events {

	public class OnNovaSenhaEnviada : EventAggregator {

		//Constantes
		private static OnNovaSenhaEnviada _instance;

		//Construtor
		public static OnNovaSenhaEnviada getInstance => _instance = _instance ?? new OnNovaSenhaEnviada();

		//Private Construtor para Singleton
		private OnNovaSenhaEnviada() {

		}

		public override void publish(object source) {
			this.publish<object>(source);
		}

		public override void subscribe(object source) {
			this.subscribe((source as OnNovaSenhaEnviadaHandler));
		}

	}
}