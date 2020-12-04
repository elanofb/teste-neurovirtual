using FluentValidation;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{

    //
    public class AssociadoReativacaoFormValidator : AbstractValidator<AssociadoReativacaoForm> {
        
        //Construtor
        public AssociadoReativacaoFormValidator() {
            
            RuleFor(x => x.motivoReativacao)
				.NotEmpty().WithMessage("Descreva aqui o motivo da reativação.")
                .MaximumLength(255).WithMessage("O texto do motivo da reativação não pode ultrapassar 255 caracteres.");
            
        }
        

    }
}
