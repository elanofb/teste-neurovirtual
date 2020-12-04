using System.Json;

namespace BLL.ConfiguracoesTextos {

	public interface IConfiguracaoTextoExclusaoBL {

		JsonMessage excluir(string key, int? idOrganizacaoParam = null);
	
	}
}