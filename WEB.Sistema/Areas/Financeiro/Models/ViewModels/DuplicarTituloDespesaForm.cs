using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.Financeiro.ViewModels
{
    // Validação da entidade titulo Despesa pagamento 
    [Validator(typeof(DuplicarTituloDespesaFormValidator))]
	public class DuplicarTituloDespesaForm {
        public TituloDespesa TituloDespesa { get; set; } 
	}
}