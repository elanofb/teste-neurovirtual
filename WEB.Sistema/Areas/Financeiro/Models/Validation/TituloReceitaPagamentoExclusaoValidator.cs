using FluentValidation;


namespace WEB.Areas.Financeiro.ViewModels{

    //
    public class TituloReceitaPagamentoExclusaoValidor : AbstractValidator<TituloReceitaExclusaoPagamentoForm> {
        
		//Atributos


		//Propriedades

        //Construtor
        public TituloReceitaPagamentoExclusaoValidor() {

            RuleFor(m => m.TituloReceitaPagamento.motivoExclusao)
                .NotEmpty().WithMessage("Informe o motivo da exclusão desse título.")
                .Length(5, 250).WithMessage("O motivo deve ter entre 5 e 250 caracteres.");
        }

    }
}
