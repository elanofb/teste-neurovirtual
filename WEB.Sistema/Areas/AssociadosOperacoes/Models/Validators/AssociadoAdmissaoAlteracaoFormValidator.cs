using System;
using FluentValidation;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{

    //
    public class AssociadoAdmissaoAlteracaoFormValidator : AbstractValidator<AssociadoAdmissaoAlteracaoForm> {
        
        //Construtor
        public AssociadoAdmissaoAlteracaoFormValidator() {
            
            RuleFor(x => x.dtAdmissao)
				.NotEmpty().WithMessage("Informe a nova data de admissão.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("A data de admissão não pode ser superior à data de hoje.");
            
        }
        

    }
}
