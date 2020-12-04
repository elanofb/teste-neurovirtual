using FluentValidation.Attributes;
using DAL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

    [Validator(typeof(ProdutoRedeFormValidator))]
	public class ProdutoRedeForm{

		public ProdutoRedeConfiguracao ProdutoRede { get; set;}

		/// <summary>
		/// Construtor
		/// </summary>
		public ProdutoRedeForm() {
			
			this.ProdutoRede = new ProdutoRedeConfiguracao();
			
		}
	}


}