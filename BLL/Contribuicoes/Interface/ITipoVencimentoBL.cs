using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public interface ITipoVencimentoBL {

		TipoVencimento carregar(int id);

		IQueryable<TipoVencimento> listar(bool? ativo);

	}
}