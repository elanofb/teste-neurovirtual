using FluentValidation;

namespace WEB.Areas.FinanceiroParcelamentos.ViewModels{

    //
    public class TituloReceitaParcelamentoValidator : AbstractValidator<TituloReceitaParcelamentoForm> {
        
		//Atributos

		//Propriedades

        //Construtor
        public TituloReceitaParcelamentoValidator() {
            
            RuleFor(x => x.listaPagamentos).SetCollectionValidator(new ParcelaValidator());

        }

    }
}
