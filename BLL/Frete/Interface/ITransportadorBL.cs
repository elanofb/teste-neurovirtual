using System.Linq;
using DAL.Frete;

namespace BLL.Frete {

	public interface ITransportadorBL {

	    Transportador carregar(int id); 

		IQueryable<Transportador> listar(string valorBusca, string ativo);
	}
}
