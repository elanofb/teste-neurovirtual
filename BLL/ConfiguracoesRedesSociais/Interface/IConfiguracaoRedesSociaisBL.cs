using System.Linq;
using DAL.ConfiguracoesRedesSociais;

namespace BLL.ConfiguracoesRedesSociais {

	public interface IConfiguracaoRedesSociaisBL {

        ConfiguracaoRedesSociais carregar(int idOrganizacao = 0, bool flagCache = true);
        
		IQueryable<ConfiguracaoRedesSociais> listar(int idOrganizacao);

        bool salvar(ConfiguracaoRedesSociais OConfiguracoes);

	}
}