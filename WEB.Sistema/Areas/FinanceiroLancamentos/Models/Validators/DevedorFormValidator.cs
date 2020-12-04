using System;
using FluentValidation;
using BLL.FinanceiroLancamentos;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels{

    //
    public class DevedorFormValidator : AbstractValidator<DevedorForm> {
        
		//Atributos
		private IDevedorBL _DevedorBL; 

		//Propriedades
		private IDevedorBL ODevedorBL => (this._DevedorBL = this._DevedorBL ?? new DevedorBL() );

        //Construtor
        public DevedorFormValidator() {            
            
			//Regras somente para pessoas físicas
			When( m => m.Devedor.Pessoa.flagTipoPessoa.Equals("F"), () => {

                RuleFor(x => x.Devedor.Pessoa.nome).NotEmpty().WithMessage("Informe o nome do Devedor.");
					
                When(x => !String.IsNullOrEmpty(x.Devedor.Pessoa.nroDocumento), () =>{
                    RuleFor(x => UtilString.onlyAlphaNumber(x.Devedor.Pessoa.nroDocumento)).Length(11).WithMessage("Um CPF deve possuir 11 caracteres.");
                    RuleFor(x => x.Devedor.Pessoa.nroDocumento).Must( UtilValidation.isCPF ).WithMessage("Informe um CPF válido.");
                    RuleFor(x => x.Devedor.Pessoa.nroDocumento).Must( (x, nroDocumento) => !this.existeDocumento(x) ).WithMessage("Este CPF já está cadastrado.");
                });
			});

			//Regras somente para pessoas jurídicas
			When( m => m.Devedor.Pessoa.flagTipoPessoa.Equals("J"), () => {

                RuleFor(x => x.Devedor.Pessoa.nome).NotEmpty().WithMessage("Informe o nome fantasia do Devedor.");

                When(x => !String.IsNullOrEmpty(x.Devedor.Pessoa.nroDocumento), () =>{
                    RuleFor(x => UtilString.onlyAlphaNumber(x.Devedor.Pessoa.nroDocumento)).Length(14).WithMessage("Um CNPJ deve possuir 14 caracteres.");
                    RuleFor(x => x.Devedor.Pessoa.nroDocumento).Must( UtilValidation.isCNPJ ).WithMessage("Informe um CNPJ válido.");
                    RuleFor(x => x.Devedor.Pessoa.nroDocumento).Must( (x, nroDocumento) => !this.existeDocumento(x) ).WithMessage("Este CNPJ já está cadastrado.");
                });
			});

			//Regra para E-mail principal
			When( m => !String.IsNullOrEmpty(m.Devedor.Pessoa.emailPrincipal), () => {
			    RuleFor(x => x.Devedor.Pessoa.emailPrincipal)
			        .EmailAddress().WithMessage("Informe um endereço de e-mail válido.");
			});

			//Regra para E-mail secundário
			When( m => !String.IsNullOrEmpty(m.Devedor.Pessoa.emailSecundario), () => {
			    RuleFor(x => x.Devedor.Pessoa.emailSecundario)
			        .EmailAddress().WithMessage("Informe um endereço de e-mail válido.");
			});
		}

		//
        private bool existeDocumento(DevedorForm ODevedorForm) {
            return this.ODevedorBL.existe(ODevedorForm.Devedor);
        }
    }
}
