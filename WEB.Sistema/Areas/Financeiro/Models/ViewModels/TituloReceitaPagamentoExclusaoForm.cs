using FluentValidation.Attributes;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{

    [Validator(typeof(TituloReceitaPagamentoExclusaoValidor))]
	public class TituloReceitaExclusaoPagamentoForm{

        public TituloReceitaPagamento TituloReceitaPagamento { get; set; } 

	}
}