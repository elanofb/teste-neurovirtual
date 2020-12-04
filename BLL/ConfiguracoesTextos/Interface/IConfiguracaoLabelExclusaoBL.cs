using System.Json;

namespace BLL.ConfiguracoesTextos {

	public interface IConfiguracaoLabelExclusaoBL {

		JsonMessage excluir(string key, int? idOrganizacaoParam = null);
	
	}
}