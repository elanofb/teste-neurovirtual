using FluentValidation.Attributes;
using DAL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

	[Validator(typeof(EstoqueSaidaFormValidator))]
	public class EstoqueSaidaForm{

		public EstoqueSaida EstoqueSaida { get; set;} 
	}


}