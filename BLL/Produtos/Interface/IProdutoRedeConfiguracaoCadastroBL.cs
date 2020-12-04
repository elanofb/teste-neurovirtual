using DAL.Produtos;

namespace BLL.Produtos {

	public interface IProdutoRedeConfiguracaoCadastroBL{
        
	    bool salvar(ProdutoRedeConfiguracao Registro);
        
	}
}