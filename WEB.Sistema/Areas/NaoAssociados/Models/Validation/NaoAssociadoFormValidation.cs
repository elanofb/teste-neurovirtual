using System;
using FluentValidation;
using BLL.Associados;
using BLL.Configuracoes;
using BLL.NaoAssociados;
using DAL.Documentos;

namespace WEB.Areas.NaoAssociados.ViewModels{

	public class NaoAssociadoFormValidator : AbstractValidator<NaoAssociadoForm> {
		
		//Atributos
		private INaoAssociadoBL _NaoAssociadoBL;

		//Propriedades
		private INaoAssociadoBL ONaoAssociadoBL => (this._NaoAssociadoBL = this._NaoAssociadoBL ?? new NaoAssociadoBL() );

	    //Construtor
		public NaoAssociadoFormValidator() {

			When(x => x.NaoAssociado.Pessoa.flagTipoPessoa == "F", () => {
				this.validarDadosPF();
				this.validarDocumentosPF();
			});

			When(x => x.NaoAssociado.Pessoa.flagTipoPessoa == "J", () => {
				this.validarDadosPJ();
			});

			this.validarDadosContato();
			this.validarDadosAcesso();
		 }

		//Validacao para dados pessoais
		private void validarDadosPF() { 

			 RuleFor(x => x.NaoAssociado.Pessoa.idPaisOrigem)
				 .NotEmpty().WithMessage("Informe o país de nascimento do não associado.");


			 RuleFor(x => x.NaoAssociado.Pessoa.nome)
				 .NotEmpty().WithMessage("Informe o nome.");

			When(x => x.NaoAssociado.Pessoa.dtNascimento != null, () => {
				 RuleFor(x => x.NaoAssociado.Pessoa.dtNascimento)
					 .Must( (x, dtNascimento) => (x.NaoAssociado.Pessoa.dtNascimento > new DateTime(1915, 1, 1) && x.NaoAssociado.Pessoa.dtNascimento < DateTime.Today))
					 .WithMessage("Informe uma data de nascimento válida.");
			});

		}

		//Validacoes para documentos do associado PF
		private void validarDocumentosPF() { 

			RuleFor(x => x.NaoAssociado.Pessoa.idTipoDocumento)
				.NotEmpty().WithMessage("Informe o tipo de documento do não associado.");

			When(x => x.NaoAssociado.Pessoa.idTipoDocumento > 0, () => {
				RuleFor(x => x.NaoAssociado.Pessoa.idTipoDocumento)
					.NotEqual(TipoDocumentoConst.CNPJ).WithMessage("Para uma pessoa física, o documento não pode ser o CNPJ.");
			});

			When(x => x.NaoAssociado.Pessoa.idPaisOrigem == "BRA", () => {
				
				 RuleFor(x => x.NaoAssociado.Pessoa.nroDocumento)
					 .NotEmpty().WithMessage("Para cadastros brasileiros é obrigatório informar o CPF.");

				When(x => (x.NaoAssociado.Pessoa.idTipoDocumento == TipoDocumentoConst.CPF && !String.IsNullOrEmpty(x.NaoAssociado.Pessoa.nroDocumento) ), () => {
					RuleFor(x => x.NaoAssociado.Pessoa.nroDocumento )
						 .Must( (x, cpf) => UtilValidation.isCPFCNPJ(x.NaoAssociado.Pessoa.nroDocumento) ).WithMessage("Informe um CPF válido.")
						 .Must( (x, cpf) => !this.existeCPF(x) ).WithMessage("Já existe um associado/não associado cadastrado com esse documento.");
				} );
			});
		}

		//Validacao para dados pessoa jurídica
		private void validarDadosPJ() { 

			 RuleFor(x => x.NaoAssociado.Pessoa.nome)
				 .NotEmpty().WithMessage("Informe o nome fantasia da empresa.");

			 RuleFor(x => x.NaoAssociado.Pessoa.idTipoDocumento)
				 .Equal( TipoDocumentoConst.CNPJ ).WithMessage("O tipo de documento da empresa deve ser um CNPJ.");

				RuleFor(x => x.NaoAssociado.Pessoa.nroDocumento)
					.NotEmpty().WithMessage("O CNPJ da empresa é uma informação obrigatória.");

			When(x => !String.IsNullOrEmpty(x.NaoAssociado.Pessoa.nroDocumento), () => {
				RuleFor(x => x.NaoAssociado.Pessoa.nroDocumento )
						.Must( (x, nroDocumento) => UtilValidation.isCPFCNPJ(x.NaoAssociado.Pessoa.nroDocumento) ).WithMessage("Informe um CNPJ válido.")
						.Must( (x, nroDocumento) => !this.existeCPF(x) ).WithMessage("Já existe um associado/não associado cadastrado com esse CNPJ.");
			} );

			When(x => x.NaoAssociado.Pessoa.dtNascimento != null, () => {
				 RuleFor(x => x.NaoAssociado.Pessoa.dtNascimento)
					 .Must( (x, dtNascimento) => (x.NaoAssociado.Pessoa.dtNascimento > new DateTime(1915, 1, 1) && x.NaoAssociado.Pessoa.dtNascimento < DateTime.Today))
					 .WithMessage("Informe uma data de fundação válida.");
			});

		}

		//Validar dados de Contato
		private void validarDadosContato() { 
			 RuleFor(x => x.NaoAssociado.Pessoa.ddiTelPrincipal)
				 .NotEmpty().WithMessage("Informe o DDI.");
				
			 RuleFor(x => x.NaoAssociado.Pessoa.dddTelPrincipal)
				 .NotEmpty().WithMessage("Informe o DDD.");
				
			 RuleFor(x => x.NaoAssociado.Pessoa.nroTelPrincipal)
				 .NotEmpty().WithMessage("Informe o Telefone.");

			RuleFor(x => x.NaoAssociado.Pessoa.emailPrincipal)
				.NotEmpty().WithMessage("O e-mail principal é obrigatório.")
				.EmailAddress().WithMessage("Informe um endereço de e-mail válido.");

            When(x => !String.IsNullOrEmpty(x.NaoAssociado.Pessoa.emailPrincipal), () =>
            {
                RuleFor(x => x.NaoAssociado.Pessoa.emailPrincipal)
                .Must((x, emailPrincipal) => !this.existeEmailPrincipal(x)).WithMessage("Já existe um associado/não associado utilizando esse e-mail (principal).");
            });

			When(x => !String.IsNullOrEmpty(x.NaoAssociado.Pessoa.emailSecundario), () =>{
				RuleFor(x => x.NaoAssociado.Pessoa.emailSecundario)
					.EmailAddress().WithMessage("Informe um endereço de e-mail válido.")
					.NotEqual(x => x.NaoAssociado.Pessoa.emailPrincipal).WithMessage("Os endereços de e-mail não podem ser iguais.");
			});

		}

		//Validar dados de acesso
		private void validarDadosAcesso() {



		}

		#region Validacoes Banco

		//Verificar existencia de CPF para evitar duplicidades  
		private bool existeCPF(NaoAssociadoForm ViewModel) { 
			int idDesconsiderado = ViewModel.NaoAssociado.id;
			bool flagExiste = this.ONaoAssociadoBL.existe(TipoDocumentoConst.CPF, ViewModel.NaoAssociado.Pessoa.nroDocumento, "", "", idDesconsiderado);
			return flagExiste;
		}

		//Verificar existencia de CPF para evitar duplicidades 
		private bool existeEmailPrincipal(NaoAssociadoForm ViewModel) { 
			int idDesconsiderado = ViewModel.NaoAssociado.id;
			bool flagExiste = this.ONaoAssociadoBL.existe(0, "", ViewModel.NaoAssociado.Pessoa.emailPrincipal, "", idDesconsiderado);
			return flagExiste;
		}

		//Verificar existencia do Login para evitar duplicidades 
		private bool existeLogin(NaoAssociadoForm ViewModel) { 
			int idDesconsiderado = ViewModel.NaoAssociado.id;
			bool flagExiste = this.ONaoAssociadoBL.existe(0, "", "", ViewModel.NaoAssociado.Pessoa.login, idDesconsiderado);
			return flagExiste;
		}
		#endregion
	}
}