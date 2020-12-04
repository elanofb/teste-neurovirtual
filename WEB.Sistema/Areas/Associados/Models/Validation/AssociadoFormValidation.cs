using System;
using FluentValidation;
using BLL.Associados;
using DAL.Associados;
using DAL.Documentos;

namespace WEB.Areas.Associados.ViewModels{

	public class AssociadoFormValidator : AbstractValidator<AssociadoForm> {
		
		//Atributos
		private IAssociadoBL _AssociadoBL;

		//Propriedades
		private IAssociadoBL OAssociadoBL{ get{ return (this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL() ); }}

		//Construtor
		public AssociadoFormValidator() {

			When(x => x.Associado.Pessoa.flagTipoPessoa == "F", () => {
				this.validarDadosPF();
				this.validarDocumentosPF();
			});

			When(x => x.Associado.Pessoa.flagTipoPessoa == "J", () => {
				this.validarDadosPJ();
			});

			this.validarDadosContato();
			this.validarDadosAcesso();
			this.validarDadosAssociado();
			this.validarResponsavelContribuicao();
		 }

		//Validacao para dados pessoais
		private void validarDadosPF() { 

			 RuleFor(x => x.Associado.Pessoa.idPaisOrigem)
				 .NotEmpty().WithMessage("Informe o país de nascimento do associado.");


			 RuleFor(x => x.Associado.Pessoa.nome)
				 .NotEmpty().WithMessage("Informe o nome do associado.");

			When(x => x.Associado.Pessoa.dtNascimento != null, () => {
				 RuleFor(x => x.Associado.Pessoa.dtNascimento)
					 .Must( (x, dtNascimento) => (x.Associado.Pessoa.dtNascimento > new DateTime(1915, 1, 1) && x.Associado.Pessoa.dtNascimento < DateTime.Today))
					 .WithMessage("Informe uma data de nascimento válida.");
			});

		}

		//Validacoes para documentos do associado PF
		private void validarDocumentosPF() { 

			RuleFor(x => x.Associado.Pessoa.idTipoDocumento)
				.NotEmpty().WithMessage("Informe o tipo de documento do associado.");

			When(x => x.Associado.Pessoa.idTipoDocumento > 0, () => {
				RuleFor(x => x.Associado.Pessoa.idTipoDocumento)
					.NotEqual(TipoDocumentoConst.CNPJ).WithMessage("Para uma pessoa física, o documento não pode ser o CNPJ.");
			});

			When(x => x.Associado.Pessoa.idPaisOrigem == "BRA", () => {
				
				 RuleFor(x => x.Associado.Pessoa.nroDocumento)
					 .NotEmpty().WithMessage("Para associados brasileiros é obrigatório informar o CPF.");

				When(x => (x.Associado.Pessoa.idTipoDocumento == TipoDocumentoConst.CPF && !String.IsNullOrEmpty(x.Associado.Pessoa.nroDocumento) ), () => {
					RuleFor(x => x.Associado.Pessoa.nroDocumento )
						 .Must( (x, cpf) => UtilValidation.isCPFCNPJ(x.Associado.Pessoa.nroDocumento) ).WithMessage("Informe um CPF válido.")
						 .Must( (x, cpf) => !this.existeCPF(x) ).WithMessage("Já existe um associado cadastrado com esse documento.");
				} );
			});
		}

		//Validacao para dados pessoa jurídica
		private void validarDadosPJ() { 

			 RuleFor(x => x.Associado.Pessoa.nome)
				 .NotEmpty().WithMessage("Informe o nome fantasia da empresa.");

			 RuleFor(x => x.Associado.Pessoa.idTipoDocumento)
				 .Equal( TipoDocumentoConst.CNPJ ).WithMessage("O tipo de documento da empresa deve ser um CNPJ.");

				RuleFor(x => x.Associado.Pessoa.nroDocumento)
					.NotEmpty().WithMessage("O CNPJ da empresa é uma informação obrigatória.");

			When(x => !String.IsNullOrEmpty(x.Associado.Pessoa.nroDocumento), () => {
				RuleFor(x => x.Associado.Pessoa.nroDocumento )
						.Must( (x, nroDocumento) => UtilValidation.isCPFCNPJ(x.Associado.Pessoa.nroDocumento) ).WithMessage("Informe um CNPJ válido.")
						.Must( (x, nroDocumento) => !this.existeCPF(x) ).WithMessage("Já existe um associado cadastrado com esse CNPJ.");
			} );

			When(x => x.Associado.Pessoa.dtNascimento != null, () => {
				 RuleFor(x => x.Associado.Pessoa.dtNascimento)
					 .Must( (x, dtNascimento) => (x.Associado.Pessoa.dtNascimento > new DateTime(1915, 1, 1) && x.Associado.Pessoa.dtNascimento < DateTime.Today))
					 .WithMessage("Informe uma data de fundação válida.");
			});

		}

		//Validar dados de Contato
		private void validarDadosContato() { 
			 RuleFor(x => x.Associado.Pessoa.ddiTelPrincipal)
				 .NotEmpty().WithMessage("Informe o DDI.");
				
			 RuleFor(x => x.Associado.Pessoa.dddTelPrincipal)
				 .NotEmpty().WithMessage("Informe o DDD.");
				
			 RuleFor(x => x.Associado.Pessoa.nroTelPrincipal)
				 .NotEmpty().WithMessage("Informe o Telefone.");

			RuleFor(x => x.Associado.Pessoa.emailPrincipal)
				.NotEmpty().WithMessage("O e-mail principal é obrigatório.")
				.EmailAddress().WithMessage("Informe um endereço de e-mail válido.");

            When(x => !String.IsNullOrEmpty(x.Associado.Pessoa.emailPrincipal), () =>
            {
                RuleFor(x => x.Associado.Pessoa.emailPrincipal)
                .Must((x, emailPrincipal) => !this.existeEmailPrincipal(x)).WithMessage("Já existe um associado utilizando esse e-mail (principal).");
            });

			When(x => !String.IsNullOrEmpty(x.Associado.Pessoa.emailSecundario), () =>{
				RuleFor(x => x.Associado.Pessoa.emailSecundario)
					.EmailAddress().WithMessage("Informe um endereço de e-mail válido.")
					.NotEqual(x => x.Associado.Pessoa.emailPrincipal).WithMessage("Os endereços de e-mail não podem ser iguais.");
					//.Must( (x, emailSecundario) => !this.existeEmailSecundario(x) ).WithMessage("Já existe um associado utilizando esse e-mail (secundário).");
			});

		}

		//Validar dados de acesso
		private void validarDadosAcesso() { 

			 RuleFor(x => x.Associado.Pessoa.login)
				 .NotEmpty().WithMessage("Informe o login de acesso para área restrita.");

             When(x => !String.IsNullOrEmpty(x.Associado.Pessoa.login), () => {

                 RuleFor(x => x.Associado.Pessoa.login)
                    .Must((x, login) => !this.existeLogin(x)).WithMessage("Já existe um associado utilizando esse login, escolha outro.");

             });				 
		}

		//Validar responsavel pelo pagamento da contribuição
		//Caso nao seja o proprio associado, a cobrança será feita para essa pessoa/empresa
		private void validarResponsavelContribuicao() { 
			
			//When(x => x.idTipoEstipulante == TipoEstipulanteConst.EMPRESA, () =>{ 
			//	RuleFor(x => x.Associado.idEmpresaEstipulante)
			//		.NotEmpty().WithMessage("Informe qual é a empresa responsável pelo pagamento da contribuição do associado (aba 2).");
			//});

		}

		#region Validacoes Banco
		//Validar dados de acesso
		private void validarDadosAssociado() { 
			 RuleFor(x => x.Associado.idTipoAssociado)
				 .NotEmpty().WithMessage("Informe qual é o tipo do associado.");
		}

		//Verificar existencia de CPF para evitar duplicidades  
		private bool existeCPF(AssociadoForm ViewModel) { 
			int idDesconsiderado = ViewModel.Associado.id;
			bool flagExiste = this.OAssociadoBL.existe(TipoDocumentoConst.CPF, ViewModel.Associado.Pessoa.nroDocumento, "", "", AssociadoTipoCadastroConst.CONSUMIDOR, idDesconsiderado);
			return flagExiste;
		}

		//Verificar existencia de CPF para evitar duplicidades 
		private bool existeEmailPrincipal(AssociadoForm ViewModel) { 
			int idDesconsiderado = ViewModel.Associado.id;
			bool flagExiste = this.OAssociadoBL.existe(0, "", ViewModel.Associado.Pessoa.emailPrincipal, "", AssociadoTipoCadastroConst.CONSUMIDOR, idDesconsiderado);
			return flagExiste;
		}

		//Verificar existencia de CPF para evitar duplicidades 
		private bool existeEmailSecundario(AssociadoForm ViewModel) { 
			int idDesconsiderado = ViewModel.Associado.id;
			bool flagExiste = this.OAssociadoBL.existe(0, "", ViewModel.Associado.Pessoa.emailSecundario, "", AssociadoTipoCadastroConst.CONSUMIDOR, idDesconsiderado);
			return flagExiste;
		}

		//Verificar existencia do Login para evitar duplicidades 
		private bool existeLogin(AssociadoForm ViewModel) { 
			int idDesconsiderado = ViewModel.Associado.id;
			bool flagExiste = this.OAssociadoBL.existe(0, "", "", ViewModel.Associado.Pessoa.login, AssociadoTipoCadastroConst.CONSUMIDOR, idDesconsiderado);
			return flagExiste;
		}
		#endregion
	}
}