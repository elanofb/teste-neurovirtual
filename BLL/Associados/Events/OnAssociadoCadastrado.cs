using BLL.Core.Events;
using BLL.Associados.Events;

namespace BLL.Associados {

	public class OnAssociadoCadastrado : EventAggregator {

		//Constantes
		private static OnAssociadoCadastrado _instance;

		//Construtor
		public static OnAssociadoCadastrado getInstance {
			get { return _instance ?? (_instance = new OnAssociadoCadastrado()); }
		}

		//Private Construtor para Singleton
		private OnAssociadoCadastrado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnAssociadoCadastradoHandler>((source as OnAssociadoCadastradoHandler));
		}
	}
}