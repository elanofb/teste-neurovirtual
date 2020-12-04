using System;
using FluentValidation;
using BLL.Mailings;

namespace WEB.Areas.Associados.ViewModels{

    //
    public class AssociadoListaEmailFormValidator : AbstractValidator<AssociadoListaEmailForm> {
        
		//Atributos
		private IMailingBL _MailingBL; 

		//Propriedades
		private IMailingBL OMailingBL => this._MailingBL = this._MailingBL ?? new MailingBL();

        //Construtor
        public AssociadoListaEmailFormValidator() {
            
            RuleFor(x => x.Mailing.idAssociado)
				.NotEmpty().WithMessage("Informe a qual associado será vinculado.");

            RuleFor(x => x.Mailing.idTipoMailing)
                .NotEmpty().WithMessage("Informe o grupo da lista.");

            RuleFor(x => x.Mailing.nome)
				.NotEmpty().WithMessage("Informe o nome.");

            RuleFor(x => x.Mailing.email)
                .NotEmpty().WithMessage("Informe o e-mail.")
                .EmailAddress().WithMessage("Informe um endereço de e-mail válido.");

            When(x => !String.IsNullOrEmpty(x.Mailing.email), () => {
                RuleFor(x => x.Mailing.email)
                .Must((x, emailPrincipal) => !this.existe(x)).WithMessage("Este e-mail já foi cadastrado.");
            });
        }

        //Verificar se o contato já existe
        public bool existe(AssociadoListaEmailForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.Mailing.id);

			return this.OMailingBL.existe(ViewModel.Mailing.email, Convert.ToInt32(ViewModel.Mailing.idTipoMailing), Convert.ToInt32(ViewModel.Mailing.idAssociado), idDesconsiderado);
        }
    }
}
