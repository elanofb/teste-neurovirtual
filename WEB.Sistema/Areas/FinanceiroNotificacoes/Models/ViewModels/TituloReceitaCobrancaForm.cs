using System.Collections.Generic;
using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.FinanceiroNotificacoes.ViewModels {

    [Validator(typeof(TituloReceitaCobrancaFormValidator))]
    public class TituloReceitaCobrancaForm {

        public string emailCobrancaTitulo { get; set; }

        public string emailCobrancaHtml { get; set; }

        public TituloReceita TituloReceita { get; set; }

        public List<int> idsTitulosReceita { get; set; }

    }
}