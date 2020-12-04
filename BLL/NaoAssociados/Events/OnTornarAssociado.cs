using BLL.Core.Events;

namespace BLL.NaoAssociados.Events {

	public class OnTornarAssociado : EventAggregator {

		//Constantes
		private static OnTornarAssociado _instance;

		//Construtor
		public static OnTornarAssociado getInstance => _instance ?? (_instance = new OnTornarAssociado());

	    //Private Construtor para Singleton
		private OnTornarAssociado() {
		}
		
		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.subscribe<OnTornarAssociadoHandler>((source as OnTornarAssociadoHandler));
		}
	}
}