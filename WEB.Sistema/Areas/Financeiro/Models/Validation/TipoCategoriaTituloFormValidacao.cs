using System;
using FluentValidation;
using BLL.Financeiro;


namespace WEB.Areas.Financeiro.ViewModels{

    //
    public class TipoCategoriaTituloValidator : AbstractValidator<TipoCategoriaTituloForm> {
        
		//Atributos
		private ITipoCategoriaTituloBL _TipoCategoriaTituloBL; 

		//Propriedades
		private ITipoCategoriaTituloBL OTipoCategoriaTituloBL { get{ return (this._TipoCategoriaTituloBL = this._TipoCategoriaTituloBL ?? new TipoCategoriaTituloBL() ); }}

        //Construtor
        public TipoCategoriaTituloValidator() {
            
            RuleFor(x => x.TipoCategoriaTitulo.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição.");

            RuleFor(x => x.TipoCategoriaTitulo.idCategoria)
				.NotEmpty()
				.WithMessage("Informe a categoria.");

			RuleFor(x =>x.TipoCategoriaTitulo.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe um registro cadastrado com essa descrição.");

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(TipoCategoriaTituloForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.TipoCategoriaTitulo.id);
			return this.OTipoCategoriaTituloBL.existe(ViewModel.TipoCategoriaTitulo.idCategoria, ViewModel.TipoCategoriaTitulo.descricao, idDesconsiderado);
        }

    }
}
