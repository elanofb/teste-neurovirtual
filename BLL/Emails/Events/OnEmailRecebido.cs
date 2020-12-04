using BLL.Core.Events;

namespace BLL.Email {

	public class OnEmailRecebido : EventAggregator {
		private static OnEmailRecebido _eventoEmailRecebido;

		//Construtor
		public static OnEmailRecebido getInstance {
			get { return _eventoEmailRecebido ?? (_eventoEmailRecebido = new OnEmailRecebido()); }
		}

		//
		private OnEmailRecebido() {
		}

		//
		public override void publish(object source) {
			this.publish<object>(source);
		}

		//
		public override void subscribe(object source) {
			this.publish<EmailRecebidoHandler>((source as EmailRecebidoHandler));
		}
	}
}