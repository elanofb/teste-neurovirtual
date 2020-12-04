using System;
using FluentValidation;
using BLL.Empresas;

namespace WEB.Areas.Empresas.ViewModels{

    //
    public class EmpresaValidator : AbstractValidator<EmpresaForm> {
        
		//Atributos
		private IEmpresaBL _EmpresaBL; 

		//Propriedades
		private IEmpresaBL OEmpresaBL { get{ return (this._EmpresaBL = this._EmpresaBL ?? new EmpresaBL() ); }}

        //Construtor
        public EmpresaValidator() {
            
            RuleFor(x => x.Empresa.Pessoa.nome).NotEmpty().WithMessage("Informe o nome fantasia da empresa.");

            RuleFor(x => x.Empresa.Pessoa.nroDocumento).NotEmpty().WithMessage("Informe o CNPJ da Empresa");

			When(x => !String.IsNullOrEmpty(x.Empresa.Pessoa.nroDocumento), () =>{
				RuleFor(x => UtilString.onlyAlphaNumber(x.Empresa.Pessoa.nroDocumento)).Length(14).WithMessage("Um CNPJ deve possuir 14 caracteres.");
				RuleFor(x => x.Empresa.Pessoa.nroDocumento).Must( (cnpj) => UtilValidation.isCPFCNPJ(cnpj) ).WithMessage("Informe um CNPJ válido para a empresa.");
				RuleFor(x => x.Empresa.Pessoa.nroDocumento).Must( (x, nroDocumento) => !this.existeCNPJ(x) ).WithMessage("Já existe uma empresa cadastrada com esse CNPJ.");
			});

			When(x => !String.IsNullOrEmpty(x.Empresa.Pessoa.emailPrincipal), () =>{
				RuleFor(x => x.Empresa.Pessoa.emailPrincipal)
					.EmailAddress().WithMessage("O e-mail informado não é válido.");

				RuleFor(x => x.Empresa.Pessoa.emailPrincipal)
					.Must( (x, email) => !this.existeEmailPrincipal(x) ).WithMessage("Esse e-mail já está sendo usado por outra empresa.");
			});

			When(x => !String.IsNullOrEmpty(x.Empresa.Pessoa.emailSecundario), () =>{
				RuleFor(x => x.Empresa.Pessoa.emailSecundario)
					.EmailAddress().WithMessage("O e-mail informado não é válido.");

				RuleFor(x => x.Empresa.Pessoa.emailSecundario)
					.Must( (x, email) => !this.existeEmailSecundario(x) ).WithMessage("Esse e-mail já está sendo usado por outra empresa.");
			});


        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existeCNPJ(EmpresaForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.Empresa.id);
			return this.OEmpresaBL.existe(ViewModel.Empresa.Pessoa.nroDocumento, "", idDesconsiderado);
        }

		//Verificar se existe outra empresa com o mesmo email
        public bool existeEmailPrincipal(EmpresaForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.Empresa.id);
			return this.OEmpresaBL.existe("", ViewModel.Empresa.Pessoa.emailPrincipal, idDesconsiderado);
        }

		//Verificar se existe outra empresa com o mesmo email
        public bool existeEmailSecundario(EmpresaForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.Empresa.id);
			return this.OEmpresaBL.existe("", ViewModel.Empresa.Pessoa.emailSecundario, idDesconsiderado);
        }

    }
}
