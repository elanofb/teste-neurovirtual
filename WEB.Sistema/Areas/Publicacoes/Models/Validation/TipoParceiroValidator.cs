using System;
using FluentValidation;
using BLL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels{

    //
    public class TipoParceiroValidator : AbstractValidator<TipoParceiroForm> {
        
		//Atributos
		private ITipoParceiroBL _TipoParceiroBL; 

		//Propriedades
		private ITipoParceiroBL OTipoParceiroBL => (this._TipoParceiroBL = this._TipoParceiroBL ?? new TipoParceiroBL() );

        //Construtor
        public TipoParceiroValidator() {
            
            RuleFor(x => x.TipoParceiro.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição.");

			RuleFor(x =>x.TipoParceiro.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe um registro cadastrado com essa descrição.");

        }

        //Validação de duplicados
        public bool existe(TipoParceiroForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.TipoParceiro.id);
            int idOrganizacao = UtilNumber.toInt32(ViewModel.TipoParceiro.idOrganizacao);
            return this.OTipoParceiroBL.existe(ViewModel.TipoParceiro.descricao, idDesconsiderado, idOrganizacao);
        }

    }
}
