using BLL.Core.Events;

namespace BLL.Email {

	public class OnEmailEnviado : EventAggregator {
		private static OnEmailEnviado _eventoEmailEnviado;

		//Construtor
		public static OnEmailEnviado getInstance {
			get { return _eventoEmailEnviado ?? (_eventoEmailEnviado = new OnEmailEnviado()); }
		}

		//Construtor
		private OnEmailEnviado() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.publish<EmailEnviadoHandler>((source as EmailEnviadoHandler));
		}
	}
}