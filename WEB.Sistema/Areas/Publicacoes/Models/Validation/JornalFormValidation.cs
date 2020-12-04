using FluentValidation;

namespace WEB.Areas.Publicacoes.ViewModels {

    //
    public class JornalFormValidation : AbstractValidator<JornalForm> {
        
        //
        public JornalFormValidation() {
            RuleFor(x => x.Jornal.titulo)
                .NotEmpty().WithMessage("Informe o título");

            RuleFor(x => x.Jornal.dtJornal)
                .NotEmpty().WithMessage("Informe a data");

            RuleFor(x => x.Jornal.ativo)
                .NotEmpty().WithMessage("Informe o status");

        }
    }
}