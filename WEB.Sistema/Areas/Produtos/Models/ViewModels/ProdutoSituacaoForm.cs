using FluentValidation.Attributes;
using DAL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

    [Validator(typeof(ProdutoSituacaoValidator))]
	public class ProdutoSituacaoForm{

		public ProdutoSituacao ProdutoSituacao { get; set;} 
	}


}