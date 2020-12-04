using System;
using FluentValidation;
using BLL.Fornecedores;

namespace WEB.Areas.Fornecedores.ViewModels{

    //
    public class FornecedorFormValidator : AbstractValidator<FornecedorForm> {
        
		//Atributos
		private IFornecedorConsultaBL _FornecedorConsultaBL; 

		//Propriedades
		private IFornecedorConsultaBL OFornecedorConsultaBL { get{ return (this._FornecedorConsultaBL = this._FornecedorConsultaBL ?? new FornecedorConsultaBL() ); }}

        //Construtor
        public FornecedorFormValidator() {            
            
			//Regras somente para pessoas físicas
			When( m => m.Fornecedor.Pessoa.flagTipoPessoa.Equals("F"), () => {

                RuleFor(x => x.Fornecedor.Pessoa.nome).NotEmpty().WithMessage("Informe o nome do fornecedor.");

                RuleFor(x => x.Fornecedor.Pessoa.nome).Must( (x, nome) => !this.existeNome(x) ).WithMessage("Já existe um fornecedor com este nome.");;
					
                When(x => !String.IsNullOrEmpty(x.Fornecedor.Pessoa.nroDocumento), () =>{
                    RuleFor(x => UtilString.onlyAlphaNumber(x.Fornecedor.Pessoa.nroDocumento)).Length(11).WithMessage("Um CPF deve possuir 11 caracteres.");
                    RuleFor(x => x.Fornecedor.Pessoa.nroDocumento).Must( (nroDocumento) => UtilValidation.isCPFCNPJ(nroDocumento) ).WithMessage("Informe um CPF válido.");
                    RuleFor(x => x.Fornecedor.Pessoa.nroDocumento).Must( (x, nroDocumento) => !this.existeDocumento(x) ).WithMessage("Este CPF já está cadastrado.");
                });
			});

			//Regras somente para pessoas juridicas
			When( m => m.Fornecedor.Pessoa.flagTipoPessoa.Equals("J"), () => {

                RuleFor(x => x.Fornecedor.Pessoa.razaoSocial).NotEmpty().WithMessage("Informe a razão social do fornecedor.");

                RuleFor(x => x.Fornecedor.Pessoa.nome).NotEmpty().WithMessage("Informe o nome fantasia do fornecedor.");

                RuleFor(x => x.Fornecedor.Pessoa.nome).Must( (x, nome) => !this.existeNome(x) ).WithMessage("Já existe um fornecedor com este nome.");;

                When(x => !String.IsNullOrEmpty(x.Fornecedor.Pessoa.nroDocumento), () =>{
                    RuleFor(x => UtilString.onlyAlphaNumber(x.Fornecedor.Pessoa.nroDocumento)).Length(14).WithMessage("Um CNPJ deve possuir 14 caracteres.");
                    RuleFor(x => x.Fornecedor.Pessoa.nroDocumento).Must( (nroDocumento) => UtilValidation.isCPFCNPJ(nroDocumento) ).WithMessage("Informe um CNPJ válido.");
                    RuleFor(x => x.Fornecedor.Pessoa.nroDocumento).Must( (x, nroDocumento) => !this.existeDocumento(x) ).WithMessage("Este CNPJ já está cadastrado.");
                });
			});

			//Regra para E-mail principal
			When( m => !String.IsNullOrEmpty(m.Fornecedor.Pessoa.emailPrincipal), () => {

				RuleFor(x => x.Fornecedor.Pessoa.emailPrincipal)
					.EmailAddress().WithMessage("Informe um endereço de e-mail válido.")
					.Must((x, emailPrincipal) => !this.existeEmail(x, x.Fornecedor.Pessoa.emailPrincipal))
                    .WithMessage("Já existe um fornecedor utilizando esse e-mail.");
			});

			//Regra para E-mail secundário
			When( m => !String.IsNullOrEmpty(m.Fornecedor.Pessoa.emailSecundario), () => {

				RuleFor(x => x.Fornecedor.Pessoa.emailSecundario)
					.EmailAddress().WithMessage("Informe um endereço de e-mail válido.")
					.Must((x, emailSecundario) => !this.existeEmail(x, x.Fornecedor.Pessoa.emailSecundario))
                    .WithMessage("Já existe um fornecedor utilizando esse e-mail.");
			});
		}

		//
        private bool existeDocumento(FornecedorForm OFornecedorForm) {
            return this.OFornecedorConsultaBL.existe(OFornecedorForm.Fornecedor);
        }

		//
        public bool existeEmail(FornecedorForm OFornecedorForm, string email) {
            return this.OFornecedorConsultaBL.existe(OFornecedorForm.Fornecedor);
        }

        //
        public bool existeNome(FornecedorForm OFornecedorForm) {
            return this.OFornecedorConsultaBL.existe(OFornecedorForm.Fornecedor);
        }
    }
}
