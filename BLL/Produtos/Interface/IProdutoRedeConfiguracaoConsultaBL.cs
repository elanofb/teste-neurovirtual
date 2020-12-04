using System.Linq;
using DAL.Produtos;

namespace BLL.Produtos {

	public interface IProdutoRedeConfiguracaoConsultaBL{
        
	    IQueryable<ProdutoRedeConfiguracao> query();

		ProdutoRedeConfiguracao carregar(int id);

		IQueryable<ProdutoRedeConfiguracao> listar(int idProduto);

	}
}