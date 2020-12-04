using FluentValidation.Attributes;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{

    [Validator(typeof(FormaPagamentoValidator))]
	public class FormaPagamentoForm{
        public FormaPagamento FormaPagamento { get; set; } 
	}
}