using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface IConfiguracaoTipoAssociadoBL {

        ConfiguracaoTipoAssociado carregar(int idTipoAssociado, int idOrganizacao = 0, bool flagCache = true);
        
		IQueryable<ConfiguracaoTipoAssociado> listar(int idTipoAssociado, int idOrganizacao);

        bool salvar(ConfiguracaoTipoAssociado OConfiguracoes);

	}
}