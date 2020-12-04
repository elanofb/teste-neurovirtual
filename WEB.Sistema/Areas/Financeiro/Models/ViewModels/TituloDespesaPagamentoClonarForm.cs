using DAL.Financeiro;
using FluentValidation;
using FluentValidation.Attributes;

namespace WEB.Areas.Financeiro.ViewModels{
    
    [Validator(typeof(TituloDespesaPagamentoClonarFormValidator))]
	public class TituloDespesaPagamentoClonarForm {
        public TituloDespesaPagamento TituloDespesaPagamento { get; set; } 
	}

    internal class TituloDespesaPagamentoClonarFormValidator : AbstractValidator<TituloDespesaPagamentoClonarForm> {

        public TituloDespesaPagamentoClonarFormValidator() {
            RuleFor(x => x.TituloDespesaPagamento.dtVencimento)
                .NotEmpty()
                .WithMessage("Informe a data de vencimento");

            RuleFor(x => x.TituloDespesaPagamento.valorOriginal)
                .NotEmpty()
                .WithMessage("Informe o valor do pagamento");
        }
    }
}