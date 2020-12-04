using System.Linq;
using DAL.ConfiguracoesScripts;

namespace BLL.ConfiguracoesScripts {

	public interface IConfiguracaoScriptsBL {

        ConfiguracaoScripts carregar(int idOrganizacao = 0, bool flagCache = true);
        
		IQueryable<ConfiguracaoScripts> listar(int idOrganizacao);

        bool salvar(ConfiguracaoScripts OConfiguracoes);

	}
}