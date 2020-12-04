using FluentValidation;

namespace WEB.Areas.Pedidos.ViewModels {

    public class PedidoCancelamentoFormValidator : AbstractValidator<PedidoCancelamentoForm> {

        public PedidoCancelamentoFormValidator() {

            RuleFor(x => x.motivoCancelamento)
                .NotEmpty().WithMessage("Informe o motivo de cancelamento do pedido.");
            
        }

    }

}