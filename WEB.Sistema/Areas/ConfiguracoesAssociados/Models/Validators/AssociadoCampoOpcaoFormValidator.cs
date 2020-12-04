using FluentValidation;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels {

    //
    public class AssociadoCampoOpcaoFormValidator : AbstractValidator<AssociadoCampoOpcaoForm> {

        //
        public AssociadoCampoOpcaoFormValidator() {

            RuleFor(x => x.AssociadoCampoOpcao.value)
                .NotEmpty().WithMessage("Informe o value da opção");

            RuleFor(x => x.AssociadoCampoOpcao.texto)
                .NotEmpty().WithMessage("Informe o texto de apresentação da opção");
        }
    }
}