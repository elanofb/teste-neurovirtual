using FluentValidation;

namespace WEB.Areas.Localizacao.ViewModels {

    //
    public class PaisValidator : AbstractValidator<PaisVM> {
		
		//
		public PaisValidator() {

			 RuleFor(x => x.Pais.id)
				 .NotEmpty().WithMessage("O campo 'ID' é obrigatório.");

			 RuleFor(x => x.Pais.nome)
				 .NotEmpty().WithMessage("O campo 'NOME' é obrigatório.");
				 
            RuleFor(x => x.Pais.idPaisBACEN)
				.NotEmpty().WithMessage("Informe qual é o código do banco central para esse país.");


		 }
        
	}
}