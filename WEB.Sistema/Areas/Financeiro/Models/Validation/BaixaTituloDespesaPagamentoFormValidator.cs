using System;
using FluentValidation;

namespace WEB.Areas.Financeiro.ViewModels
{
    
    public class BaixaTituloDespesaPagamentoFormValidator : AbstractValidator<BaixaTituloDespesaPagamentoForm>
    {

        public BaixaTituloDespesaPagamentoFormValidator()
        {
            RuleFor(x => x.TituloDespesaPagamento.idMeioPagamento)
                .NotEmpty().WithMessage("Informe o meio de pagamento utilizado.");

            RuleFor(x => x.TituloDespesaPagamento.dtPagamento)
				.NotEmpty().WithMessage("Informe a data em que o pagamento foi realizado")
				.LessThanOrEqualTo(DateTime.Now).WithMessage("A data de pagamento não pode estar no futuro.");
        }
    }
}
