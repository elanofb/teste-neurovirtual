using FluentValidation;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{

    //
    public class AssociadoSituacaoContribuicaoFormValidator : AbstractValidator<AssociadoSituacaoContribuicaoForm> {
        
        //Construtor
        public AssociadoSituacaoContribuicaoFormValidator() {
            
            RuleFor(x => x.motivoAlteracao)
               .NotEmpty().WithMessage("Informe motivo da alteração.");

        }
        
    }
}
