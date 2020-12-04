using DAL.Produtos;
using PagedList;

namespace WEB.Areas.Produtos.ViewModels{
    
	public class ProdutoComposicaoVM{

		public IPagedList<ProdutoComposicao> listaProdutoComposicao { get; set;}

        public int idProduto { get; set; }

        public ProdutoComposicaoVM() {
        }
	}
}