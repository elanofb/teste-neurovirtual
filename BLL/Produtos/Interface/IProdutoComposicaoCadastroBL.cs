using System.Json;
using DAL.Produtos;

namespace BLL.Produtos {

	public interface IProdutoComposicaoCadastroBL{
        
	    bool salvar(ProdutoComposicao OProdutoComposicao);
        
	    JsonMessageStatus alterarStatus(int id);
        
	}
}