using System.Linq;
using DAL.ConfiguracoesRecibo;

namespace BLL.ConfiguracoesRecibo {

	public interface IConfiguracaoReciboBL {

        ConfiguracaoRecibo carregar(int idOrganizacao = 0, bool flagCache = true);
        
		IQueryable<ConfiguracaoRecibo> listar(int idOrganizacao);

        bool salvar(ConfiguracaoRecibo OConfiguracoes);

	}
}