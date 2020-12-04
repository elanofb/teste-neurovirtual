using FluentValidation;

namespace WEB.Areas.Pedidos.ViewModels {

    public class PedidoAcaoCancelamentoFormValidator : AbstractValidator<PedidoAcaoCancelamentoForm> {

        public PedidoAcaoCancelamentoFormValidator() {

            RuleFor(x => x.motivoCancelamento)
                .NotEmpty().WithMessage("Informe o motivo de cancelamento do pedido.");
            
        }

    }

}