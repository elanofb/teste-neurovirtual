using System.Json;
using DAL.Produtos;

namespace BLL.Produtos {

	public interface IProdutoItemCadastroBL{
        
	    bool salvar(ProdutoItem OProdutoItem);
        
	    JsonMessageStatus alterarStatus(int id);
        
	}
}