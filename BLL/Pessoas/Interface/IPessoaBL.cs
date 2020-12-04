using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

	public interface IPessoaBL {

		IQueryable<Pessoa> query(int? idOrganizacaoParam = null);
		
        Pessoa carregar(int id);

		IQueryable<Pessoa> listar(string valorBusca, string ativo);

        bool existe(string descricao, int id);

		Pessoa existe(string valor);

		bool excluir(int[] ids);

	}
}