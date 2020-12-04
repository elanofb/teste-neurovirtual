using System.Linq;
using DAL.ConfiguracoesEcommerce;

namespace BLL.ConfiguracoesEcommerce {

	public interface IConfiguracaoEcommerceBL {

        ConfiguracaoEcommerce carregar(int idOrganizacao = 0, bool flagCache = true);
        
		IQueryable<ConfiguracaoEcommerce> listar(int idOrganizacao);

        bool salvar(ConfiguracaoEcommerce OConfiguracoes);

	}
}