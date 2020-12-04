using System.Linq;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados {

	public interface IConfiguracaoAssociadoPFBL {

        ConfiguracaoAssociadoPF carregar(int idOrganizacao = 0, bool flagCache = true);
        
		IQueryable<ConfiguracaoAssociadoPF> listar(int idOrganizacao);

        bool salvar(ConfiguracaoAssociadoPF OConfiguracoes);

	}
}