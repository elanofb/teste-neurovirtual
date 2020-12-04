using BLL.Core.Events;

namespace BLL.UsuariosInternos.Events {

	public class OnUsuarioInternoCadastrado : EventAggregator {

		private static OnUsuarioInternoCadastrado _evento;

		//Construtor
		public static OnUsuarioInternoCadastrado getInstance => _evento ?? (_evento = new OnUsuarioInternoCadastrado());

	    //
		private OnUsuarioInternoCadastrado() {

		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object handler) {
			this.subscribe((handler as OnUsuarioInternoCadastradoHandler));
		}
	}
}