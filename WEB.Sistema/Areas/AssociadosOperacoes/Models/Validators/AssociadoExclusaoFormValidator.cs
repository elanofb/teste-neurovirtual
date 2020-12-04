using FluentValidation;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{

    //
    public class AssociadoExclusaoFormValidator : AbstractValidator<AssociadoExclusaoForm> {
        
        //Construtor
        public AssociadoExclusaoFormValidator() {
            
            RuleFor(x => x.idMotivoDesligamento)
                .NotEmpty().WithMessage("Informe o motivo do desligamento.");

            RuleFor(x => x.motivoExclusao)
				.NotEmpty().WithMessage("Descreva aqui o motivo do desligamento.")
                .MaximumLength(255).WithMessage("O texto do motivo de desligamento não pode ultrapassar 255 caracteres.");
            
        }
        

    }
}
