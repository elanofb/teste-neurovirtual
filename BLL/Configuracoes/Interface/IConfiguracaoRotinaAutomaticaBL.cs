using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public interface IConfiguracaoRotinaAutomaticaBL {

        ConfiguracaoRotinaAutomatica carregar(int idTransportadora, bool flagCache = true);

        IQueryable<ConfiguracaoRotinaAutomatica> listar(int idTransportadora);

        bool salvar(ConfiguracaoRotinaAutomatica OConfiguracoes);
	}
}