using System.Json;

namespace BLL.Contatos {

	public interface ITipoContatoExclusaoBL {

		JsonMessage excluir(int[] ids);

	}
	
}