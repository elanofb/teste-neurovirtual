using FluentValidation;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{

    //
    public class AssociadoTipoAlteracaoFormValidator : AbstractValidator<AssociadoTipoAlteracaoForm> {
        
        //Construtor
        public AssociadoTipoAlteracaoFormValidator() {
            
            RuleFor(x => x.idTipoAssociado)
				.NotEmpty().WithMessage("Informe o novo tipo de associado.");
            
        }
        

    }
}
