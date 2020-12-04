using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public interface IConfiguracaoComissaoBL {

        ConfiguracaoComissao carregar(int idTransportadora = 0, bool flagCache = true);

        IQueryable<ConfiguracaoComissao> listar(int idTransportadora);

        bool salvar(ConfiguracaoComissao OConfiguracoes);
	}
}