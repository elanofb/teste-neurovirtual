using System.Linq;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados {

	public interface IConfiguracaoAssociadoPJBL {

        ConfiguracaoAssociadoPJ carregar(int idOrganizacao = 0, bool flagCache = true);
        
		IQueryable<ConfiguracaoAssociadoPJ> listar(int idOrganizacao);

        bool salvar(ConfiguracaoAssociadoPJ OConfiguracoes);

	}
}