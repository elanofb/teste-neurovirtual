using System.Json;
using DAL.Relacionamentos;

namespace BLL.Relacionamentos {

	public interface IOcorrenciaRelacionamentoCadastroBL {

		bool salvar(OcorrenciaRelacionamento OTipoContato);

		JsonMessageStatus alterarStatus(int id);

	}
	
}