using BLL.Permissao;
using FluentValidation;

namespace WEB.Areas.Permissao.ViewModels{

    //
    public class RecursoGrupoValidator : AbstractValidator<RecursoGrupoForm> {
        
		//Atributos
		private IAcessoRecursoGrupoBL _AcessoRecursoGrupoBL; 

		//Propriedades
		private IAcessoRecursoGrupoBL OAcessoRecursoGrupoBL => this._AcessoRecursoGrupoBL = this._AcessoRecursoGrupoBL ?? new AcessoRecursoGrupoBL();

        //Construtor
        public RecursoGrupoValidator() {
            
            RuleFor(x => x.AcessoRecursoGrupo.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição do Tipo de Produto.");

        }


    }
}
