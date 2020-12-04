using FluentValidation.Attributes;
using DAL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

    [Validator(typeof(TipoProdutoValidator))]
	public class TipoProdutoForm{

		public TipoProduto TipoProduto { get; set;} 
	}


}