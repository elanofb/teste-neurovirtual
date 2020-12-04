using FluentValidation;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{

    //
    public class AssociadoEnvioSenhaTransacaoValidator : AbstractValidator<EnvioSenhaTransacao> {
        
        //Construtor
        public AssociadoEnvioSenhaTransacaoValidator() {
            
            RuleFor(x => x.novaSenha)
                .NotEmpty().WithMessage("O campo de \"nova senha de transação\" deve ser preenchido.")
                .Length(6).WithMessage("A senha de transação deve ter 6 caracteres.");


        }
        

    }
}
