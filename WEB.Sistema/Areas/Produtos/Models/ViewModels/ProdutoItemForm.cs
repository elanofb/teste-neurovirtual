using FluentValidation.Attributes;
using DAL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

    [Validator(typeof(ProdutoItemValidator))]
	public class ProdutoItemForm{

		public ProdutoItem ProdutoItem { get; set;} 
	}


}