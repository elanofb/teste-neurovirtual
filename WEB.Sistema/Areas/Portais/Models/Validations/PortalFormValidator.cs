using System;
using System.Linq.Dynamic;
using FluentValidation;
using BLL.Portais;

namespace WEB.Areas.Portais.ViewModels{

    //
    public class PortalFormValidator : AbstractValidator<PortalForm> {
        
		//Atributos
		private IPortalBL _PortalBL; 

		//Propriedades
		private IPortalBL OPortalBL => (this._PortalBL = this._PortalBL ?? new PortalBL() );

        //Construtor
        public PortalFormValidator() {


            RuleFor(x => x.Portal.descricao)
               .NotEmpty()
               .WithMessage("Informe o nome do banco.");

            RuleFor(x => x.Portal.descricao)
                    .Must((x,nome) => !this.existe(x))
                    .WithMessage("Já existe um banco cadastrado com esse nome.");

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(PortalForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.Portal.id);
			return this.OPortalBL.existe(ViewModel.Portal.descricao, idDesconsiderado).Any();
        }
    }
}
