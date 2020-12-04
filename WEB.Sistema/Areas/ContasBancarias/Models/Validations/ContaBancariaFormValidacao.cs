using FluentValidation;

namespace WEB.Areas.ContasBancarias.ViewModels{

    //
    public class ContaBancariaValidator : AbstractValidator<ContaBancariaForm> {
        
        //Construtor
        public ContaBancariaValidator() {

            RuleFor(x => x.ContaBancaria.descricao)
                .NotEmpty().WithMessage("Informe o Titulo");

            RuleFor(x => x.ContaBancaria.idBanco)
                .NotEmpty().WithMessage("Informe o Banco");

            RuleFor(x => x.ContaBancaria.saldoInicial)
                .NotEmpty().WithMessage("Informe o saldo inicial da conta.");

        }
    }
}
