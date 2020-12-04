using FluentValidation;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels {

    //
    public class AssociadoCampoFormValidator : AbstractValidator<AssociadoCampoForm> {

        //Atributos

        //Propriedades

        //
        public AssociadoCampoFormValidator() {

            RuleFor(x => x.AssociadoCampo.idAssociadoCampoGrupo)
                .NotEmpty().WithMessage("Informe grupo ao qual o campo deve pertencer.");

            RuleFor(x => x.AssociadoCampo.idTipoCampo)
                .NotEmpty().WithMessage("Informe o tipo do campo");

            RuleFor(x => x.AssociadoCampo.label)
                .NotEmpty().WithMessage("Informe qual será o label para a descrição");

            RuleFor(x => x.AssociadoCampo.name)
                .NotEmpty().WithMessage("Informe qual será o 'name' para a descrição");
        }

        //
    }
}