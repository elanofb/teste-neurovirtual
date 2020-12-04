using System;
using FluentValidation;
using BLL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{

    //
    public class CentroCustoValidator : AbstractValidator<CentroCustoForm> {
        
		//Atributos
		private ICentroCustoBL _CentroCustoBL; 

		//Propriedades
		private ICentroCustoBL OCentroCustoBL { get{ return (this._CentroCustoBL = this._CentroCustoBL ?? new CentroCustoBL() ); }}

        //Construtor
        public CentroCustoValidator() {
            
            RuleFor(x => x.CentroCusto.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição.");

			RuleFor(x =>x.CentroCusto.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe um registro cadastrado com essa descrição.");

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(CentroCustoForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.CentroCusto.id);
			return this.OCentroCustoBL.existe(ViewModel.CentroCusto.descricao, idDesconsiderado);
        }

    }
}
