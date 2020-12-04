using BLL.Core.Events;
using System;
using DAL.Financeiro;

namespace BLL.FinanceiroLancamentos.Events {

    public class OnDespesaCadastradaHandler : IHandler<object> {

        //Chamador das ações do evento
        public void execute(object source) {

            var OTituloDespesa = source as TituloDespesa;

        }
    }
}