using FluentValidation;

namespace WEB.Areas.Financeiro.ViewModels
{
    
    public class DuplicarTituloDespesaFormValidator : AbstractValidator<DuplicarTituloDespesaForm>
    {

        public DuplicarTituloDespesaFormValidator()
        {
            //RuleFor(x => x.TituloDespesaPagamento.idFormaPagamento)
            //    .NotEmpty().WithMessage("Informe qual foi a forma de pagamento utilizada.");

            //RuleFor(x => x.TituloDespesaPagamento.dtPagamento)
            //    .NotEmpty().WithMessage("Informe a data em que o pagamento foi realizado")
            //    .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de pagamento não pode estar no futuro.");
        }
    }
}
