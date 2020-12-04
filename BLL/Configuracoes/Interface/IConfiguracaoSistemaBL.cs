using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public interface IConfiguracaoSistemaBL{

        ConfiguracaoSistema carregar(int idTransportadora = 0, bool flagCache = true);

        ConfiguracaoSistema carregarParaApi(string codigoOrganizacao);

        ConfiguracaoSistema carregarPadrao();

        IQueryable<ConfiguracaoSistema> listar(int idOrganizacao);

        bool salvar(ConfiguracaoSistema OConfiguracoes);
	}
}