using System.Json;
using DAL.Produtos;

namespace BLL.Produtos {

	public interface IProdutoSituacaoCadastroBL
	{
        
	    bool salvar(ProdutoSituacao OProdutoSituacao);
        
	    JsonMessageStatus alterarStatus(int id);
        
	}
}