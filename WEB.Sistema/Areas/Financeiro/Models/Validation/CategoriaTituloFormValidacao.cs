using System;
using FluentValidation;
using BLL.Financeiro;


namespace WEB.Areas.Financeiro.ViewModels{

    //
    public class CategoriaTituloValidator : AbstractValidator<CategoriaTituloForm> {
        
		//Atributos
		private ICategoriaTituloBL _CategoriaTituloBL; 

		//Propriedades
		private ICategoriaTituloBL OCategoriaTituloBL { get{ return (this._CategoriaTituloBL = this._CategoriaTituloBL ?? new CategoriaTituloBL() ); }}

        //Construtor
        public CategoriaTituloValidator() {
            
            RuleFor(x => x.CategoriaTitulo.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição da Categoria.");

			RuleFor(x =>x.CategoriaTitulo.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe uma Categoria cadastrada com essa descrição.");

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(CategoriaTituloForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.CategoriaTitulo.id);
			return this.OCategoriaTituloBL.existe(ViewModel.CategoriaTitulo.idMacroConta, ViewModel.CategoriaTitulo.descricao, idDesconsiderado);
        }

    }
}
