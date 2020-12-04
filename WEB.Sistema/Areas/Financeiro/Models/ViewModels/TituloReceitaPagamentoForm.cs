using FluentValidation.Attributes;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{

    [Validator(typeof (TituloReceitaPagamentoValidator))]
    public class TituloReceitaPagamentoForm {

        public TituloReceitaPagamento TituloReceitaPagamento { get; set; }
    }
}