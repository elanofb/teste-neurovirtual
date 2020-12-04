using System.Linq;
using DAL.ConfiguracoesCateirinha;

namespace BLL.ConfiguracoesCarteirinha {

	public interface IConfiguracaoCarteirinhaBL {

        ConfiguracaoCarteirinha carregar(int idOrganizacao = 0, bool flagCache = true);
        
		IQueryable<ConfiguracaoCarteirinha> listar(int idOrganizacao);

        bool salvar(ConfiguracaoCarteirinha OConfiguracoes);

	}
}