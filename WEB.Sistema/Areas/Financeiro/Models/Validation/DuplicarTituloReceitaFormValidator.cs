using FluentValidation;

namespace WEB.Areas.Financeiro.ViewModels
{
    
    public class DuplicarTituloReceitaFormValidator : AbstractValidator<DuplicarTituloReceitaForm>
    {

        public DuplicarTituloReceitaFormValidator()
        {
            //RuleFor(x => x.TituloReceitaPagamento.idFormaPagamento)
            //    .NotEmpty().WithMessage("Informe qual foi a forma de pagamento utilizada.");

            //RuleFor(x => x.TituloReceitaPagamento.dtPagamento)
            //    .NotEmpty().WithMessage("Informe a data em que o pagamento foi realizado")
            //    .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de pagamento não pode estar no futuro.");
        }
    }
}
