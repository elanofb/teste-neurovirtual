using System;
using System.Security.Principal;
using FluentValidation;
using BLL.Associados;
using BLL.ConfiguracoesAssociados;
using DAL.Associados;
using DAL.ConfiguracoesAssociados;
using DAL.Documentos;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Associados.ViewModels{

	public class AssociadoCadastroPFFormValidator : AbstractValidator<AssociadoCadastroPFForm> {
		
		//Atributos
		private IAssociadoBL _AssociadoBL;		
		
		//Propriedades
		private IAssociadoBL OAssociadoBL => this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL();		
		
        private ConfiguracaoAssociadoPF OConfigPF => ConfiguracaoAssociadoPFBL.getInstance.carregar();
		
		private IPrincipal User => HttpContextFactory.Current.User;
		
	    //Construtor
		public AssociadoCadastroPFFormValidator() {

		    When(x => !x.Associado.Pessoa.nroDocumento.isEmpty(), () => {
				
				RuleFor(x => x.Associado.Pessoa.nroDocumento)
					.Must( (x, nroDocumento) => UtilValidation.isCPF(nroDocumento)).WithMessage("Informe um CPF válido.");


                if (OConfigPF.flagPermitirCPFDuplicado != true) {
                    RuleFor(x => x.Associado.Pessoa.nroDocumento)
                        .Must( (x, nroDocumento) => !this.existeCPF(x)).WithMessage("O CPF informado já está sendo usado por outro associado.");
                }
			    
		    });

			When(x => x.Associado.id == 0, () => {
												RuleFor(x => x.Associado.codigoIndicador)
												.Must( (x, codigoIndicador) => this.existeIndicador(x)).WithMessage("O código do indicador informado não é de um membro ativo no sistema.");
											});
			
			RuleFor(x => x.Associado.rotaConta)
				.Must( (x, rotaConta) => !this.existeRota(x)).WithMessage("O link informado já está sendo usado por outro membro.");
                
		}
		
        private bool existeCPF(AssociadoCadastroPFForm ViewModel) {
			
            var nroDocumento = UtilString.onlyNumber(ViewModel.Associado.Pessoa.nroDocumento);
			
            var flagExiste = this.OAssociadoBL.existe(TipoDocumentoConst.CPF, nroDocumento, "", "", AssociadoTipoCadastroConst.CONSUMIDOR, ViewModel.Associado.id);
			
            return flagExiste;
        }
		
		private bool existeIndicador(AssociadoCadastroPFForm ViewModel) {
			
			var codigoIndicador = ViewModel.Associado.codigoIndicador;
			
			bool flagExiste = this.OAssociadoBL.existeCodigo(codigoIndicador, ViewModel.Associado.id);
			
			return flagExiste;
			
		}
		
		private bool existeRota(AssociadoCadastroPFForm ViewModel) {
			
			string rota = ViewModel.Associado.rotaConta;
			
			bool flagExiste = this.OAssociadoBL.existeRota(rota, ViewModel.Associado.id, User.idOrganizacao());						
			
			return flagExiste;
			
		}
		
	}
}