using FluentValidation;

namespace WEB.Areas.Pedidos.ViewModels{

	public class PedidoCadastroFormValidator : AbstractValidator<PedidoCadastroForm> {
		
		public PedidoCadastroFormValidator() {

            RuleFor(x => x.Pedido.idPessoa)       
                .NotEmpty().WithMessage("Informe quem é o comprador!");
			
	    }

	}

}