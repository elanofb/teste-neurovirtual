using System.Json;
using DAL.Representantes;

namespace BLL.Representantes {

	public interface IRepresentanteCadastroBL {
		
        bool salvar(Representante ORepresentante);
		
		JsonMessageStatus alterarStatus(int id);
		
	}
	
}
