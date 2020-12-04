using System;
using System.Security.Principal;
using FluentValidation;
using BLL.Associados;
using BLL.ConfiguracoesAssociados;
using DAL.Associados;
using DAL.ConfiguracoesAssociados;
using DAL.Documentos;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.NaoAssociados.ViewModels{

	public class NaoAssociadoCadastroPJFormValidator : AbstractValidator<NaoAssociadoCadastroPJForm> {
		
		//Atributos
		private IAssociadoBL _AssociadoBL;

		//Propriedades
		private IAssociadoBL OAssociadoBL => this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL();
        private ConfiguracaoAssociadoPJ OConfigPJ => ConfiguracaoAssociadoPJBL.getInstance.carregar();
		
		private IPrincipal User => HttpContextFactory.Current.User;

	    //Construtor
		public NaoAssociadoCadastroPJFormValidator() {

		    When(x => !x.Associado.Pessoa.nroDocumento.isEmpty(), () => {

				RuleFor(x => x.Associado.Pessoa.nroDocumento)
					.Must( (x, nroDocumento) => UtilValidation.isCNPJ(nroDocumento)).WithMessage("Informe um CNPJ válido.");


                if (OConfigPJ.flagPermitirCNPJDuplicado != true) {

                    RuleFor(x => x.Associado.Pessoa.nroDocumento)
                        .Must( (x, nroDocumento) => !this.existeCNPJ(x)).WithMessage("O CNPJ informado já está sendo usado por outro membro.");
                }

		    });

			When(x => x.Associado.id == 0, () => {
												RuleFor(x => x.Associado.codigoIndicador)
												.Must( (x, codigoIndicador) => this.existeIndicador(x)).WithMessage("O código do indicador informado não é de um membro ativo no sistema.");
											});
			
			RuleFor(x => x.Associado.rotaConta)
				.Must( (x, rotaConta) => !this.existeRota(x)).WithMessage("O link informado já está sendo usado por outro membro.");
                
		}
		
        private bool existeCNPJ(NaoAssociadoCadastroPJForm ViewModel) {

            var nroDocumento = UtilString.onlyNumber(ViewModel.Associado.Pessoa.nroDocumento);

            var flagExiste = this.OAssociadoBL.existe(TipoDocumentoConst.CNPJ, nroDocumento, "", "", AssociadoTipoCadastroConst.COMERCIANTE, ViewModel.Associado.id);

            return flagExiste;

        }
		
		private bool existeIndicador(NaoAssociadoCadastroPJForm ViewModel) {
			
			var codigoIndicador = ViewModel.Associado.codigoIndicador;
			
			bool flagExiste = this.OAssociadoBL.existeCodigo(codigoIndicador, ViewModel.Associado.id);
			
			return flagExiste;
			
		}
		
		private bool existeRota(NaoAssociadoCadastroPJForm ViewModel) {
			
			string rota = ViewModel.Associado.rotaConta;
			
			bool flagExiste = this.OAssociadoBL.existeRota(rota, ViewModel.Associado.id, User.idOrganizacao());						
			
			return flagExiste;
			
		}
        

	}
}