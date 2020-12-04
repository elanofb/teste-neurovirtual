using FluentValidation;
using System;

namespace WEB.Areas.AssociadosCarteirinha.ViewModels{

    //
    public class AssociadosCarteirinhaFormValidator : AbstractValidator<AssociadosCarteirinhaForm> {
        
		//Atributos

		//Propriedades

        //Construtor
        public AssociadosCarteirinhaFormValidator() {
            
            RuleFor(x => x.AssociadoCarteirinha.idAssociado)
                .NotEmpty().WithMessage("Informe a quem a ocorrência deve estar vinculada.");
            
            RuleFor(x => x.AssociadoCarteirinha.dtEnvio)
                .NotEmpty().WithMessage("Informe a data de envio da carteirinha.");

			RuleFor(x => x.AssociadoCarteirinha.flagTipoEnvio)
                .NotEmpty().WithMessage("Informe o tipo de envio da carteirinha.");

            RuleFor(x => x.AssociadoCarteirinha.flagTipoEmissao)
                .NotEmpty().WithMessage("Informe o tipo de emissão da carteirinha.");
            
            RuleFor(x => x.AssociadoCarteirinha.observacao)
                .Must((x, obs) => x.AssociadoCarteirinha.observacao.Length <= 1000 ).WithMessage("As observações não podem ter mais de 1000 caracteres.")
                .When(x => !String.IsNullOrEmpty(x.AssociadoCarteirinha.observacao));
            
        }

    }
}
