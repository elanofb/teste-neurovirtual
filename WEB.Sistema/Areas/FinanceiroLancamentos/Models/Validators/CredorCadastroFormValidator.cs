using System;
using FluentValidation;
using BLL.FinanceiroLancamentos;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels{

    //
    public class CredorCadastroFormValidator : AbstractValidator<CredorCadastroForm> {
        
		//Atributos
		private ICredorBL _CredorBL; 

		//Propriedades
		private ICredorBL OCredorBL => (this._CredorBL = this._CredorBL ?? new CredorBL() );

        //Construtor
        public CredorCadastroFormValidator() {            
            
			//Regras somente para pessoas físicas
			When( m => m.Credor.Pessoa.flagTipoPessoa.Equals("F"), () => {

                RuleFor(x => x.Credor.Pessoa.nome).NotEmpty().WithMessage("Informe o nome do Credor.");
					
                When(x => !String.IsNullOrEmpty(x.Credor.Pessoa.nroDocumento), () =>{
                    RuleFor(x => UtilString.onlyAlphaNumber(x.Credor.Pessoa.nroDocumento)).Length(11).WithMessage("Um CPF deve possuir 11 caracteres.");
                    RuleFor(x => x.Credor.Pessoa.nroDocumento).Must( UtilValidation.isCPF ).WithMessage("Informe um CPF válido.");
                    RuleFor(x => x.Credor.Pessoa.nroDocumento).Must( (x, nroDocumento) => !this.existeDocumento(x) ).WithMessage("Este CPF já está cadastrado.");
                });
			});

			//Regras somente para pessoas jurídicas
			When( m => m.Credor.Pessoa.flagTipoPessoa.Equals("J"), () => {

                RuleFor(x => x.Credor.Pessoa.nome).NotEmpty().WithMessage("Informe o nome fantasia do Credor.");

                When(x => !String.IsNullOrEmpty(x.Credor.Pessoa.nroDocumento), () =>{
                    RuleFor(x => UtilString.onlyAlphaNumber(x.Credor.Pessoa.nroDocumento)).Length(14).WithMessage("Um CNPJ deve possuir 14 caracteres.");
                    RuleFor(x => x.Credor.Pessoa.nroDocumento).Must( UtilValidation.isCNPJ ).WithMessage("Informe um CNPJ válido.");
                    RuleFor(x => x.Credor.Pessoa.nroDocumento).Must( (x, nroDocumento) => !this.existeDocumento(x) ).WithMessage("Este CNPJ já está cadastrado.");
                });
			});

			//Regra para E-mail principal
			When( m => !String.IsNullOrEmpty(m.Credor.Pessoa.emailPrincipal), () => {
			    RuleFor(x => x.Credor.Pessoa.emailPrincipal)
			        .EmailAddress().WithMessage("Informe um endereço de e-mail válido.");
			});

			//Regra para E-mail secundário
			When( m => !String.IsNullOrEmpty(m.Credor.Pessoa.emailSecundario), () => {
			    RuleFor(x => x.Credor.Pessoa.emailSecundario)
			        .EmailAddress().WithMessage("Informe um endereço de e-mail válido.");
			});
		}

		//
        private bool existeDocumento(CredorCadastroForm oCredorCadastroForm) {
            return this.OCredorBL.existe(oCredorCadastroForm.Credor);
        }
    }
}
