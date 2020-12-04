using System.Linq;
using DAL.Fornecedores;

namespace BLL.Fornecedores {

	public interface IFornecedorConsultaBL {
		
	    IQueryable<Fornecedor> query(int? idOrganizacaoParam = null);

		Fornecedor carregar(int id);

	    IQueryable<Fornecedor> listar(string valorBusca, bool? ativo);

		object autocompletar(string term, int idFornecedor);

		bool existe(Fornecedor OFornecedor);

	}
}
