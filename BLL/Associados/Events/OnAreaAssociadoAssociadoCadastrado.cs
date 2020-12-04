using BLL.Core.Events;
using BLL.Associados.Events;

namespace BLL.Associados {

	public class OnAreaAssociadoAssociadoCadastrado : EventAggregator {

		//Constantes
		private static OnAreaAssociadoAssociadoCadastrado _instance;

		//Construtor
		public static OnAreaAssociadoAssociadoCadastrado getInstance {
			get { return _instance ?? (_instance = new OnAreaAssociadoAssociadoCadastrado()); }
		}

		//Private Construtor para Singleton
		private OnAreaAssociadoAssociadoCadastrado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnAreaAssociadoAssociadoCadastradoHandler>((source as OnAreaAssociadoAssociadoCadastradoHandler));
		}
	}
}