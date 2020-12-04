using System;
using System.Linq;
using BLL.Ajudas;
using FluentValidation;

namespace WEB.Areas.Ajudas.ViewModels{
    
    //
	public class AjudaCategoriaFormValidator : AbstractValidator<AjudaCategoriaForm> {

        private IAjudaCategoriaBL _AjudaCategoriaBL;
        private IAjudaCategoriaBL OAjudaCategoriaBL => this._AjudaCategoriaBL = this._AjudaCategoriaBL ?? new AjudaCategoriaBL();

        public AjudaCategoriaFormValidator() {

            RuleFor(x => x.AjudaCategoria.descricao)
                .NotEmpty().WithMessage("Informe a descrição.");
            
            When(x => !String.IsNullOrEmpty(x.AjudaCategoria.descricao), () => {
                RuleFor(x => x.AjudaCategoria.descricao)
                        .Must((x, descricao) => !this.existe(x)).WithMessage("Já existe uma categoria cadastrada com essa descrição.");
            });
            
        }
        
        #region Validacoes Banco
        //Verificar existência de CNPJ para evitar duplicidades  
        private bool existe(AjudaCategoriaForm ViewModel) {
            int idDesconsiderado = ViewModel.AjudaCategoria.id;

            bool flagExiste = this.OAjudaCategoriaBL.listar("", null)
                                                    .Any(x => x.descricao == ViewModel.AjudaCategoria.descricao && x.id != idDesconsiderado);

            return flagExiste;
        }
        #endregion

    }
}