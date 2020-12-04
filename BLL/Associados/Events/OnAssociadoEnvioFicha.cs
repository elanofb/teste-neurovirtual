using BLL.Core.Events;
using BLL.Associados.Events;

namespace BLL.Associados {

	public class OnAssociadoEnvioFicha : EventAggregator {

		//Constantes
		private static OnAssociadoEnvioFicha _instance;

		//Construtor
		public static OnAssociadoEnvioFicha getInstance {
			get { return _instance ?? (_instance = new OnAssociadoEnvioFicha()); }
		}

		//Private Construtor para Singleton
		private OnAssociadoEnvioFicha() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnAssociadoEnvioFichaHandler>((source as OnAssociadoEnvioFichaHandler));
		}
	}
}