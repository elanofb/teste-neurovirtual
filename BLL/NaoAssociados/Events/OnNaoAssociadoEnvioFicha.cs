using BLL.Core.Events;
using BLL.NaoAssociados.Events;

namespace BLL.NaoAssociados {

	public class OnNaoAssociadoEnvioFicha : EventAggregator {

		//Constantes
		private static OnNaoAssociadoEnvioFicha _instance;

		//Construtor
		public static OnNaoAssociadoEnvioFicha getInstance => _instance ?? (_instance = new OnNaoAssociadoEnvioFicha());

	    //Private Construtor para Singleton
		private OnNaoAssociadoEnvioFicha() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnNaoAssociadoEnvioFichaHandler>((source as OnNaoAssociadoEnvioFichaHandler));
		}
	}
}