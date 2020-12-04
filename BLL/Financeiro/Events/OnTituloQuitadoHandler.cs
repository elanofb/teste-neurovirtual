using BLL.Core.Events;
using System;

namespace BLL.Financeiro.Events {

	public class OnTituloQuitadoHandler : IHandler<object> {

		//Atributos

		//Propridades

		//Chamador das ações do evento
		public void execute(object source) {

			try {
			    //TituloReceita OTitulo = (TituloReceit.a)source;

			} catch (Exception ex) {
				UtilLog.saveError(ex, "Erro no manipulador de evento: OnTituloQuitadoHandler");
			}
		}
	}
}