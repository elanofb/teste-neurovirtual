using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.Financeiro.ViewModels
{
    // Validação da entidade titulo Receita pagamento 
    [Validator(typeof(DuplicarTituloReceitaFormValidator))]
	public class DuplicarTituloReceitaForm {
        public TituloReceita TituloReceita { get; set; } 
	}
}