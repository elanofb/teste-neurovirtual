using FluentValidation;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{

    //
    public class AssociadoDesativacaoFormValidator : AbstractValidator<AssociadoDesativacaoForm> {
        
        //Construtor
        public AssociadoDesativacaoFormValidator() {
            
            RuleFor(x => x.idMotivoDesativacao)
                .NotEmpty().WithMessage("Informe o motivo da desativação.");

            RuleFor(x => x.motivoDesativacao)
				.NotEmpty().WithMessage("Descreva aqui o motivo da desativação.")
                .MaximumLength(255).WithMessage("O texto do motivo da desativação não pode ultrapassar 255 caracteres.");
            
        }
        

    }
}
