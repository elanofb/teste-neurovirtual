using DAL.Contribuicoes;
using FluentValidation.Attributes;

namespace WEB.Areas.ContribuicoesPainel.ViewModels {

    [Validator(typeof(ContribuicaoEnvioCobrancaFormValidator))]
    public class ContribuicaoEnvioCobrancaForm {

        //Atributos

        //Propriedades
        public ContribuicaoCobranca ContribuicaoCobranca { get; set; }

        //Construtor
        public ContribuicaoEnvioCobrancaForm() {

            ContribuicaoCobranca = new ContribuicaoCobranca();

        }

   }
}