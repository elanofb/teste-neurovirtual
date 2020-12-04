using System.Json;
using DAL.Contatos;

namespace BLL.Contatos {

	public interface ITipoContatoCadastroBL {

		bool salvar(TipoContato OTipoContato);

		JsonMessageStatus alterarStatus(int id);

	}
	
}