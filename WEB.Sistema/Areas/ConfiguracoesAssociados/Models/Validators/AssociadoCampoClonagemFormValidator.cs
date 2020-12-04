using FluentValidation;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels {

    //
    public class AssociadoCampoClonagemFormValidator : AbstractValidator<AssociadoCampoClonagemForm> {

        //Atributos

        //Propriedades

        //
        public AssociadoCampoClonagemFormValidator() {

            RuleFor(x => x.idTipoAssociadoOrigem)
                .NotEmpty().WithMessage("Informe o tipo de associado para clonar.");

            RuleFor(x => x.idsTiposAssociadoDestinos)
                .NotEmpty().WithMessage("Informe os tipos de associado que receberam os dados clonados");
            
        }

        //
    }
}