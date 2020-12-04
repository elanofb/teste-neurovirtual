using System;
using FluentValidation;
using BLL.Financeiro;


namespace WEB.Areas.Financeiro.ViewModels{

    //
    public class DetalheTipoCategoriaTituloValidator : AbstractValidator<DetalheTipoCategoriaTituloForm> {
        
		//Atributos
		private IDetalheTipoCategoriaTituloBL _DetalheTipoCategoriaTituloBL; 

		//Propriedades
		private IDetalheTipoCategoriaTituloBL ODetalheTipoCategoriaTituloBL { get{ return (this._DetalheTipoCategoriaTituloBL = this._DetalheTipoCategoriaTituloBL ?? new DetalheTipoCategoriaTituloBL() ); }}

        //Construtor
        public DetalheTipoCategoriaTituloValidator() {
            
            RuleFor(x => x.DetalheTipoCategoriaTitulo.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição.");

            RuleFor(x => x.DetalheTipoCategoriaTitulo.idTipoCategoria)
				.NotEmpty()
				.WithMessage("Informe o Tipo de Categoria.");

			RuleFor(x =>x.DetalheTipoCategoriaTitulo.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe um registro cadastrado com essa descrição.");

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(DetalheTipoCategoriaTituloForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.DetalheTipoCategoriaTitulo.id);
			return this.ODetalheTipoCategoriaTituloBL.existe(ViewModel.DetalheTipoCategoriaTitulo.idTipoCategoria, ViewModel.DetalheTipoCategoriaTitulo.descricao, idDesconsiderado);
        }

    }
}
