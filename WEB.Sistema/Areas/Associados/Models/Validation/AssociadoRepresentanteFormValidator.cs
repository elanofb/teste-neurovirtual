using System;
using FluentValidation;
using BLL.Associados;

namespace WEB.Areas.Associados.ViewModels{

    //
    public class AssociadoRepresentanteFormValidator : AbstractValidator<AssociadoRepresentanteForm> {
        
		//Atributos
		private IAssociadoRepresentanteBL _AssociadoRepresentanteBL; 

		//Propriedades
		private IAssociadoRepresentanteBL OAssociadoRepresentanteBL { get{ return (this._AssociadoRepresentanteBL = this._AssociadoRepresentanteBL ?? new AssociadoRepresentanteBL() ); }}

        //Construtor
        public AssociadoRepresentanteFormValidator() {
            
            RuleFor(x => x.AssociadoRepresentante.idAssociado)
				.NotEmpty().WithMessage("Informe a qual associado será vinculado o Representante.");

            RuleFor(x => x.AssociadoRepresentante.idTipoAssociadoRepresentante)
				.GreaterThan(0).WithMessage("Informe qual é o tipo de representante.");

            RuleFor(x => x.AssociadoRepresentante.flagRepresentantaAssociacao)
				.NotEmpty().WithMessage("Informe se ele representa a empresa perante a associação.");

            RuleFor(x => x.AssociadoRepresentante.nome)
				.NotEmpty().WithMessage("Informe o nome do representante.");

            RuleFor(x => x.AssociadoRepresentante.cpf)
				.NotEmpty().WithMessage("Informe o CPF do representante.");

			When(x => !String.IsNullOrEmpty(x.AssociadoRepresentante.cpf), () => {
			    RuleFor(x => x.AssociadoRepresentante.cpf)
			        .Must((x, nroDocumento) => UtilValidation.isCPFCNPJ(x.AssociadoRepresentante.cpf))
			        .WithMessage("Informe um CPF válido.");
			} );

			 RuleFor(x => x.AssociadoRepresentante.ddiTelPrincipal)
				 .NotEmpty().WithMessage("Informe o DDI.");
				
			 RuleFor(x => x.AssociadoRepresentante.dddTelPrincipal)
				 .NotEmpty().WithMessage("Informe o DDD.");
				
			 RuleFor(x => x.AssociadoRepresentante.nroTelPrincipal)
				 .NotEmpty().WithMessage("Informe o Telefone.");

        }

        //Verificar se o contato já existe
        public bool existe(AssociadoRepresentanteForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.AssociadoRepresentante.id);
			return this.OAssociadoRepresentanteBL.existe(ViewModel.AssociadoRepresentante, idDesconsiderado);
        }
    }
}
