using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public interface IConfiguracaoEmailBL {

        ConfiguracaoEmail carregar(int idTransportadora = 0, bool flagCache = true);

        IQueryable<ConfiguracaoEmail> listar(int idTransportadora);

        bool salvar(ConfiguracaoEmail OConfiguracoes);
	}
}