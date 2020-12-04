using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public interface IConfiguracaoNotificacaoBL {

        ConfiguracaoNotificacao carregar(int idOrganizacao = 0, bool flagCache = true);

        IQueryable<ConfiguracaoNotificacao> listar(int idOrganizacao);

        bool salvar(ConfiguracaoNotificacao OConfiguracoes);
	}
}