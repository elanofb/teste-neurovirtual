using FluentValidation;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels {

    //
    public class AssociadoCampoGrupoFormValidator : AbstractValidator<AssociadoCampoGrupoForm> {

        //Atributos

        //Propriedades

        //
        public AssociadoCampoGrupoFormValidator() {

            RuleFor(x => x.AssociadoCampoGrupo.descricao)
                .NotEmpty().WithMessage("Informe o nome do grupo");
        }

        //
    }
}