using BLL.Core.Events;
using System;
using DAL.Financeiro;

namespace BLL.Financeiro.Events {

	public class OnTituloCadastradoHandler : IHandler<object> {

		//Atributos

		//Propridades
        private ITituloReceitaBL OTituloReceitaBL => new TituloReceitaPadraoBL();

		//Chamador das ações do evento
		public void execute(object source) {

			try {

                var TituloReceita = (source as TituloReceita);

                //OTituloReceitaBL.atualizarDadosPessoa(TituloReceita.id);

			} catch (Exception ex) {

                try {
                    var TituloReceita = (source as TituloReceita);
                    UtilLog.saveError(ex, $"Erro no manipulador de evento: OnTituloCadastradoHandler, TituloReceita: {TituloReceita.id}");

                } catch (Exception subEx) {
                    UtilLog.saveError(subEx, "Erro no manipulador de evento: OnTituloCadastradoHandler, não foi possível reconhecer o source");
                }
			}
		}
	}
}