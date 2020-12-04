using FluentValidation;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels {

    //
    public class AssociadoCampoPropriedadeFormValidator : AbstractValidator<AssociadoCampoPropriedadeForm> {

        //
        public AssociadoCampoPropriedadeFormValidator() {

            RuleFor(x => x.AssociadoCampoPropriedade.nome)
                .NotEmpty().WithMessage("Informe o nome da propriedade");

            RuleFor(x => x.AssociadoCampoPropriedade.valor)
                .NotEmpty().WithMessage("Informe o valor da propriedade");
        }
    }
}