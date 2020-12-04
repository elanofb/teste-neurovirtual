using System.Linq;
using DAL.Financeiro;
using System.Json;

namespace BLL.Financeiro {

	public interface ITipoDespesaConsultaBL {

		IQueryable<TipoDespesa> query();
		
		TipoDespesa carregar(int id);

		IQueryable<TipoDespesa> listar();
		
	}
}
