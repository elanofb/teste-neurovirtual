using FluentValidation.Attributes;
using DAL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

	[Validator(typeof(EstoqueEntradaFormValidator))]
	public class EstoqueEntradaForm{

		public EstoqueEntrada EstoqueEntrada { get; set;} 
	}


}