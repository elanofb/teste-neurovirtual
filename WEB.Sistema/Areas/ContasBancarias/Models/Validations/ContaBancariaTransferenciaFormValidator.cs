using FluentValidation;

namespace WEB.Areas.ContasBancarias.ViewModels{

    //
    public class ContaBancariaTransferenciaFormValidator : AbstractValidator<ContaBancariaTransferenciaForm> {
        
        //Construtor
        public ContaBancariaTransferenciaFormValidator() {

            RuleFor(x => x.ContaBancariaMovimentacao.idContaBancariaOrigem)
                .NotEmpty().WithMessage("Informe a conta bancária de origem.");
            
            RuleFor(x => x.ContaBancariaMovimentacao.idContaBancariaDestino)
                .NotEmpty().WithMessage("Informe a conta bancária destino.");

            When(x => x.ContaBancariaMovimentacao.idContaBancariaDestino > 0, () => {

                RuleFor(x => x.ContaBancariaMovimentacao.idContaBancariaDestino)
                    .Must((ViewModel, idContaBancariaDestino) => idContaBancariaDestino != ViewModel.ContaBancariaMovimentacao.idContaBancariaOrigem)
                    .WithMessage("A conta bancária destino não pode ser a mesma conta bancária de origem.");

            });

            RuleFor(x => x.ContaBancariaMovimentacao.valor)
                .GreaterThan(0).WithMessage("Informe o valor da transferência.");

            RuleFor(x => x.ContaBancariaMovimentacao.dtOperacao)
                .NotEmpty().WithMessage("Informe a data da trnasferência.");

        }
    }
}
