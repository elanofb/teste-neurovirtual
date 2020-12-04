using System.Linq;
using DAL.FinanceiroLancamentos;

namespace BLL.FinanceiroLancamentos {

	public interface IDevedorVWBL {
		
		DevedorVW carregar(int id);

		IQueryable<DevedorVW> listar(string valorBusca);
	}
}
