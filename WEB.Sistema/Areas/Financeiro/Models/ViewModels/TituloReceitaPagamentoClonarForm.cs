using DAL.Financeiro;
using FluentValidation;
using FluentValidation.Attributes;

namespace WEB.Areas.Financeiro.ViewModels{
    
    [Validator(typeof(TituloReceitaPagamentoClonarFormValidator))]
	public class TituloReceitaPagamentoClonarForm {
        public TituloReceitaPagamento TituloReceitaPagamento { get; set; } 
	}

    internal class TituloReceitaPagamentoClonarFormValidator : AbstractValidator<TituloReceitaPagamentoClonarForm> {

        public TituloReceitaPagamentoClonarFormValidator() {
            RuleFor(x => x.TituloReceitaPagamento.dtVencimento)
                .NotEmpty()
                .WithMessage("Informe a data de vencimento");

            RuleFor(x => x.TituloReceitaPagamento.valorOriginal)
                .NotEmpty()
                .WithMessage("Informe o valor do pagamento");
        }
    }
}