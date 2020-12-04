using System.Json;

namespace BLL.Relacionamentos {

	public interface IOcorrenciaRelacionamentoExclusaoBL {

		JsonMessage excluir(int[] ids);

	}
	
}