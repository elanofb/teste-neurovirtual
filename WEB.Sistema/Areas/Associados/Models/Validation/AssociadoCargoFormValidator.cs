using System;
using FluentValidation;
using BLL.Associados;

namespace WEB.Areas.Associados.ViewModels{

    //
    public class AssociadoCargoFormValidator : AbstractValidator<AssociadoCargoForm> {
        
		//Atributos
		private IAssociadoCargoBL _AssociadoCargoBL; 

		//Propriedades
		private IAssociadoCargoBL OAssociadoCargoBL { get{ return (this._AssociadoCargoBL = this._AssociadoCargoBL ?? new AssociadoCargoBL() ); }}

        //Construtor
        public AssociadoCargoFormValidator() {
            
            RuleFor(x => x.AssociadoCargo.idAssociado)
				.NotEmpty().WithMessage("Informe a qual associado será vinculado o cargo.");

            RuleFor(x => x.AssociadoCargo.idCargo)
				.GreaterThan(0).WithMessage("Informe qual é o cargo.");

            RuleFor(x => x.AssociadoCargo.inicioGestao)
				.NotEmpty().WithMessage("Informe o início da gestão em que o cargo foi/será exercido.");

            RuleFor(x => x.AssociadoCargo.fimGestao)
				.NotEmpty().WithMessage("Informe o fim da gestão em que o cargo foi/será exercido.");

			When(x => !String.IsNullOrEmpty(x.AssociadoCargo.inicioGestao), () => {
				
				RuleFor(x => UtilDate.cast(x.AssociadoCargo.inicioGestao))
					.GreaterThan(new DateTime(1920, 1, 1)).WithMessage("Informe uma data válida para o ínicio da gestão.");

				RuleFor(x => UtilDate.cast(x.AssociadoCargo.inicioGestao))
					.LessThan( x => UtilDate.cast(x.AssociadoCargo.fimGestao) ).WithMessage("A data de início da gestão deve ser inferior ao fim.");
			} );

			When(x => (!String.IsNullOrEmpty(x.AssociadoCargo.inicioGestao) && !String.IsNullOrEmpty(x.AssociadoCargo.fimGestao)), () => {
	            RuleFor(x => x.AssociadoCargo.idCargo)
					.Must((x, idCargo) => !this.existe(x) ).WithMessage("Já há um registro cadastrado com essas informações.");
			});

        }

        //Verificar se o contato já existe
        public bool existe(AssociadoCargoForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.AssociadoCargo.id);
			return this.OAssociadoCargoBL.existe(ViewModel.AssociadoCargo, idDesconsiderado);
        }

    }
}
