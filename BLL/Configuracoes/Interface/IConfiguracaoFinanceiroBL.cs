using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public interface IConfiguracaoFinanceiroBL {

        ConfiguracaoFinanceiro carregar(int idTransportadora = 0, bool flagCache = true);

        IQueryable<ConfiguracaoFinanceiro> listar(int idTransportadora);

        bool salvar(ConfiguracaoFinanceiro OConfiguracoes);
	}
}