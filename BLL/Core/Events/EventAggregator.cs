using System.Collections.Generic;

namespace BLL.Core.Events {

	public abstract class EventAggregator {
		
		//Proriedades
		private readonly Dictionary<string, object> _subscribers = new Dictionary<string, object>();

		/// <summary>
		/// Construtor
		/// </summary>
		protected EventAggregator() {
		}

		//Deve ser herdado e implementado
		public abstract void publish(object source);
		
		/// <summary>
		/// 
		/// </summary>
		public void publish<T>(T message) {
			
			foreach (var subscriber in _subscribers.Values) {

				if (subscriber is IHandler<T> handler) {
					
					handler.execute(message); 
					
				}
			}
		}

		//Deve ser herdado e implementado
		public abstract void subscribe(object source);
		
		//
		public void subscribe<T>(T subscriber) {
			string typeSubscribe = typeof(T).Name;

			if (!_subscribers.ContainsKey(typeSubscribe)) {
				_subscribers.Add(typeSubscribe, subscriber);
			}
		}
	}
}