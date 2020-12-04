using FluentValidation.Attributes;
using DAL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

    [Validator(typeof(ProdutoComposicaoValidator))]
	public class ProdutoComposicaoForm{

		public ProdutoComposicao ProdutoComposicao { get; set;}

        public ProdutoComposicaoForm() {
            this.ProdutoComposicao = new ProdutoComposicao();
        }
	}
}