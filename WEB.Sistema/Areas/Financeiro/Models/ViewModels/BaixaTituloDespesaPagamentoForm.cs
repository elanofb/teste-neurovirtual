using System.Collections.Generic;
using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.Financeiro.ViewModels
{
    // Validação da entidade titulo Despesa pagamento 
    [Validator(typeof(BaixaTituloDespesaPagamentoFormValidator))]
	public class BaixaTituloDespesaPagamentoForm {
        public TituloDespesaPagamento TituloDespesaPagamento { get; set; } 

        public List<TituloDespesaPagamento> listaTituloDespesaPagamento { get; set; }
    }
}