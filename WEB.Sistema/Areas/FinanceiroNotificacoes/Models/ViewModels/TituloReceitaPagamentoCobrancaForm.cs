using System.Collections.Generic;
using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.FinanceiroNotificacoes.ViewModels {

    [Validator(typeof(TituloReceitaPagamentoCobrancaFormValidator))]
    public class TituloReceitaPagamentoCobrancaForm {

        public string emailCobrancaTitulo { get; set; }

        public string emailCobrancaHtml { get; set; }

        public TituloReceitaPagamento TituloReceita { get; set; }

        public List<int> idsPagamentos { get; set; }

    }
}