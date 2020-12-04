using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public interface ITipoGeracaoContribuicaoBL {

		TipoGeracaoContribuicao carregar(int id);

		IQueryable<TipoGeracaoContribuicao> listar(bool? ativo);

	}
}