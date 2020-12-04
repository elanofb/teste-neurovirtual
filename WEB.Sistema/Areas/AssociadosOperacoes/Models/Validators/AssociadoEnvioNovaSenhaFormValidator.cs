using FluentValidation;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{

    //
    public class AssociadoEnvioNovaSenhaFormValidator : AbstractValidator<AssociadoEnvioNovaSenhaForm> {
        
        //Construtor
        public AssociadoEnvioNovaSenhaFormValidator() {
            
            RuleFor(x => x.novaSenha)
                .NotEmpty().WithMessage("O campo de \"nova senha\" deve ser preenchido.")
                .MinimumLength(4).WithMessage("A senha de acesso deve ter no mínimo 4 caracteres.");


        }
        

    }
}
