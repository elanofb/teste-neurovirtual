using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

	public interface IPessoaVWBL {

		IQueryable<PessoaVW> query(int? idOrganizacaoParam = null);
		
	    PessoaVW carregar(int idPessoa);

        IQueryable<PessoaVW> listar(string valorBusca, int? idOrganizacaoInf = null);

	}
}
