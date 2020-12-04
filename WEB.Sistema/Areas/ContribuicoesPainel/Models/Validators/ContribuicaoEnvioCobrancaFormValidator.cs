using FluentValidation;

namespace WEB.Areas.ContribuicoesPainel.ViewModels {

    //
    public class ContribuicaoEnvioCobrancaFormValidator : AbstractValidator<ContribuicaoEnvioCobrancaForm> {

        //Atributos

        //Propriedades

        //
        public ContribuicaoEnvioCobrancaFormValidator() {

            RuleFor(x => x.ContribuicaoCobranca.flagSomenteVencidos)
                .NotEmpty().WithMessage("Informe quem deve ser cobrado.");
        }


    }
}