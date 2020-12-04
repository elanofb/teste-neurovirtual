using BLL.Core.Events;
using System;
using DAL.Financeiro;

namespace BLL.FinanceiroLancamentos.Events {

    public class OnReceitaCadastradaHandler : IHandler<object> {

        //Chamador das ações do evento
        public void execute(object source) {

            var OTituloReceita = source as TituloReceita;

        }
    }
}