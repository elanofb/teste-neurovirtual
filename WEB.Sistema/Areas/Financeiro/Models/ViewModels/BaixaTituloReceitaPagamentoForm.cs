using System.Collections.Generic;
using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.Financeiro.ViewModels
{
    // Validação da entidade titulo Despesa pagamento 
    [Validator(typeof(BaixaTituloReceitaPagamentoFormValidator))]
	public class BaixaTituloReceitaPagamentoForm : BaixaPagamentoAbstract {

        public override TituloReceitaPagamento TituloReceitaPagamento { get; set; }
        
        public List<TituloReceitaPagamento> listaTituloReceitaPagamento { get; set; }
        
    }
}