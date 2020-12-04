using System;
using FluentValidation;
using BLL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels{

    //
    public class CategoriaNoticiaValidator : AbstractValidator<CategoriaNoticiaForm> {
        
		//Atributos
		private ICategoriaNoticiaBL _CategoriaNoticiaBL; 

		//Propriedades
		private ICategoriaNoticiaBL OCategoriaNoticiaBL => (this._CategoriaNoticiaBL = this._CategoriaNoticiaBL ?? new CategoriaNoticiaBL() );

        //Construtor
        public CategoriaNoticiaValidator() {
            
            RuleFor(x => x.CategoriaNoticia.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição.");

			RuleFor(x =>x.CategoriaNoticia.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe um registro cadastrado com essa descrição.");

        }

        //Validação de duplicados
        public bool existe(CategoriaNoticiaForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.CategoriaNoticia.id);
            int idPortal = UtilNumber.toInt32(ViewModel.CategoriaNoticia.idPortal);
            int idOrganizacao = UtilNumber.toInt32(ViewModel.CategoriaNoticia.idOrganizacao);
            return this.OCategoriaNoticiaBL.existe(ViewModel.CategoriaNoticia.descricao, idDesconsiderado, idPortal, idOrganizacao);
        }

    }
}
