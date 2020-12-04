using FluentValidation;

namespace WEB.Areas.Financeiro.ViewModels{

    //
    public class TituloImpostoValidator : AbstractValidator<TituloImpostoForm> {
        
        //Construtor
        public TituloImpostoValidator() {
            
            RuleFor(x => x.idTabelaImposto)
				.NotEmpty()
				.WithMessage("Informe a tabela para calcular os impostos.");

        }

    }
}
