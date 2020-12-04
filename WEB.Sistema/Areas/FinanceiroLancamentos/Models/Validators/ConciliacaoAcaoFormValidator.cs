using FluentValidation;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels{

    //
    public class ConciliacaoAcaoFormValidator : AbstractValidator<ConciliacaoAcaoForm> {
        
        //Construtor
        public ConciliacaoAcaoFormValidator() {
            
            RuleFor(x => x.dtConciliacao)
                .NotEmpty().WithMessage("Informe a data de conciliação.");
        }
    }
}
