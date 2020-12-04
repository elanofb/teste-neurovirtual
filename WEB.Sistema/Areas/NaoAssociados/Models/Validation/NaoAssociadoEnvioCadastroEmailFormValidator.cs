using System;
using FluentValidation;
using System.Linq;

namespace WEB.Areas.NaoAssociados.ViewModels{

    //
    public class NaoAssociadoEnvioCadastroEmailFormValidator : AbstractValidator<NaoAssociadoEnvioCadastroEmailForm> {
        
		//Atributos

		//Propriedades

        //Construtor
        public NaoAssociadoEnvioCadastroEmailFormValidator() {
            
            RuleFor(x => x.emailsDestino)
				.NotEmpty().WithMessage("Informe ao menos um email para enviar a ficha de cadastro.")
                .Must((x , emailsDestino) => verificarEmailsValidos(x)).WithMessage("Um ou mais emails informados são inválidos.")
                .Must((x , emailsDestino) => verificarLimiteEmails(x)).WithMessage("O número de e-mails destino não pode ser maior do que 5.");

        }
        
        //Verificar validade de emails informados
        public bool verificarEmailsValidos(NaoAssociadoEnvioCadastroEmailForm ViewModel) {
            var emails = UtilString.notNull(ViewModel.emailsDestino).Split(';').Where(x => !String.IsNullOrEmpty(x)).ToList();
            var emailsValidos = emails.Where(x => UtilValidation.isEmail(x));

            return emails.Count() == emailsValidos.Count();
        }

        //Verificar limite de emails informados
        public bool verificarLimiteEmails(NaoAssociadoEnvioCadastroEmailForm ViewModel) {
            var emails = UtilString.notNull(ViewModel.emailsDestino).Split(';').Where(x => !String.IsNullOrEmpty(x)).ToList();
            var emailsValidos = emails.Where(x => UtilValidation.isEmail(x)).ToList();

            return emailsValidos.Count <= 5;
        }

    }
}
