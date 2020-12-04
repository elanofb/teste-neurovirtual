using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public interface IConfiguracaoOperacaoCompraBL {
			
        ConfiguracaoOperacaoCompra carregar(int idOrganizacao = 0, bool flagCache = true);

        IQueryable<ConfiguracaoOperacaoCompra> listar(int idOrganizacao);

        bool salvar(ConfiguracaoOperacaoCompra OConfiguracoes);
	}
}