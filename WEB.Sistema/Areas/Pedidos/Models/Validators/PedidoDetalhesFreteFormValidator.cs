using FluentValidation;

namespace WEB.Areas.Pedidos.ViewModels {

    public class PedidoDetalhesFreteFormValidator : AbstractValidator<PedidoDetalhesFreteForm> {

        public PedidoDetalhesFreteFormValidator() {

            RuleFor(x => x.PedidoEntrega.cepOrigem)
                .NotEmpty().WithMessage("O CEP de origem deve ser informado para o cálculo de frete.");

            RuleFor(x => x.PedidoEntrega.cep)
                .NotEmpty().WithMessage("O CEP de entrega deve ser informado para o cálculo de frete.");
            
        }

    }

}