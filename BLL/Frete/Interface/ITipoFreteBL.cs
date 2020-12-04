using System.Linq;
using DAL.Frete;

namespace BLL.Frete {

	public interface ITipoFreteBL {

		TipoFrete carregar(int id); 

		IQueryable<TipoFrete> listar(string valorBusca, bool? ativo);
	}
}
