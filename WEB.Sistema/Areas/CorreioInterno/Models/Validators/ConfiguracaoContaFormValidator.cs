using FluentValidation;

namespace WEB.Areas.CorreioInterno.ViewModels {

    //
    public class ConfiguracaoContaFormValidator : AbstractValidator<ConfiguracaoContaForm> {

        //Atributos

        //Propriedades

        //
        public ConfiguracaoContaFormValidator() {

            RuleFor(x => x.ConfiguracaoEmailUsuario.contaEmailSistema)
                .NotEmpty().WithMessage("Informe a conta de e-mail que será utilizada.");
            
            RuleFor(x => x.ConfiguracaoEmailUsuario.senhaEmailSistema)
                .NotEmpty().WithMessage("Informe a senha do e-mail que será utilizado.");
            
        }


    }
}