using System.Json;
using DAL.Fornecedores;

namespace BLL.Fornecedores {

	public interface IFornecedorCadastroBL {
		
		bool salvar(Fornecedor OFornecedor);

        JsonMessageStatus alterarStatus(int id);

	}
}
