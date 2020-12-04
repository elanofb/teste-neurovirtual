using System;
using FluentValidation;
using BLL.MateriaisApoio;

namespace WEB.Areas.MateriaisApoio.ViewModels{

    //
    public class TipoMaterialApoioValidator : AbstractValidator<TipoMaterialApoioForm> {
        
		//Atributos
		private ITipoMaterialApoioBL _TipoMaterialApoioBL; 

		//Propriedades
		private ITipoMaterialApoioBL OTipoMaterialApoioBL { get{ return (this._TipoMaterialApoioBL = this._TipoMaterialApoioBL ?? new TipoMaterialApoioBL() ); }}

        //Construtor
        public TipoMaterialApoioValidator() {
            
            RuleFor(x => x.TipoMaterialApoio.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição do Tipo de Material de Apoio.");

			RuleFor(x =>x.TipoMaterialApoio.descricao)
					.Must( (x, descricao) => !this.existe(x) )
					.WithMessage("Já existe um Tipo de Material de Apoio cadastrado com essa descrição.");

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(TipoMaterialApoioForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.TipoMaterialApoio.id);
			return this.OTipoMaterialApoioBL.existe(ViewModel.TipoMaterialApoio.descricao, idDesconsiderado);
        }

    }
}
