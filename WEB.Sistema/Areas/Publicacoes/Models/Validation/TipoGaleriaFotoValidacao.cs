using System;
using FluentValidation;
using BLL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels{

    //
    public class TipoGaleriaFotoValidator : AbstractValidator<TipoGaleriaFotoForm> {
        
		//Atributos
		private ITipoGaleriaFotoBL _TipoGaleriaFotoBL; 

		//Propriedades
		private ITipoGaleriaFotoBL OTipoGaleriaFotoBL => (this._TipoGaleriaFotoBL = this._TipoGaleriaFotoBL ?? new TipoGaleriaFotoBL() );

        //Construtor
        public TipoGaleriaFotoValidator() {
            
            RuleFor(x => x.TipoGaleriaFoto.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição.");

			RuleFor(x =>x.TipoGaleriaFoto.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe um registro cadastrado com essa descrição.");

        }

        //Validação de duplicados
        public bool existe(TipoGaleriaFotoForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.TipoGaleriaFoto.id);
			return this.OTipoGaleriaFotoBL.existe(ViewModel.TipoGaleriaFoto.descricao, idDesconsiderado);
        }

    }
}
