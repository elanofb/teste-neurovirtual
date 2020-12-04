using System;
using FluentValidation;

namespace WEB.Areas.Associados.ViewModels{

    //
    public class AssociadoAdmissaoFormValidator : AbstractValidator<AssociadoAdmissaoForm> {
        
        //Construtor
        public AssociadoAdmissaoFormValidator() {
            
            RuleFor(x => x.dtAdmissao)
				.NotEmpty().WithMessage("Informe a data em que o associado foi admitido.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("A data de admissão não pode ser superior a data de hoje.");
            
        }
        

    }
}
