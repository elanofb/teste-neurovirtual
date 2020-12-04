using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public interface IConfiguracaoContribuicaoBL {

        ConfiguracaoContribuicao carregar(int idTransportadora = 0, bool flagCache = true);

        IQueryable<ConfiguracaoContribuicao> listar(int idTransportadora);

        bool salvar(ConfiguracaoContribuicao OConfiguracoes);
	}
}