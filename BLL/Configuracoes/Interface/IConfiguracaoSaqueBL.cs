using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public interface IConfiguracaoSaqueBL {
			
        ConfiguracaoSaque carregar(int idOrganizacao = 0, int idTipoCadastro = 0);
		
        IQueryable<ConfiguracaoSaque> listar(int idOrganizacao);
		
        bool salvar(ConfiguracaoSaque OConfiguracoes);
		
	}
}