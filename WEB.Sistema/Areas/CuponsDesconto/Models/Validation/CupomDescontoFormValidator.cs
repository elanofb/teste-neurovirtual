using FluentValidation;

namespace WEB.Areas.CuponsDesconto.ViewModels {

    //
    public class CupomDescontoFormValidator : AbstractValidator<CupomDescontoForm> {
        //
        public CupomDescontoFormValidator() {

            RuleFor(x => x.CupomDesconto.valorDesconto).NotEmpty().WithMessage("Informe o valor de desconto.");
            RuleFor(x => x.CupomDesconto.ativo).NotEmpty().WithMessage("Informe o status.");

            When(x => x.CupomDesconto.qtdeUsos < 1 || x.CupomDesconto.qtdeUsos == null, () => {
                RuleFor(x => x.CupomDesconto.nome).NotEmpty().WithMessage("Informe o nome do beneficiado.");
            });

            RuleFor(x => x.CupomDesconto.flagPedido).Must((x, flag) => validarLocalUso(x)).WithMessage("Informe ao menos uma utilização");
        }

        private static bool validarLocalUso(CupomDescontoForm ViewModel) {
            if (!ViewModel.CupomDesconto.flagPedido && !ViewModel.CupomDesconto.flagEvento && !ViewModel.CupomDesconto.flagContribuicao) {
                return false;
            }
            return true;
        }
    }
}