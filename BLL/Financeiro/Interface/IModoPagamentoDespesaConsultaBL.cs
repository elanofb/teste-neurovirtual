using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro {

	public interface IModoPagamentoDespesaConsultaBL {

		IQueryable<ModoPagamentoDespesa> query();
		
		ModoPagamentoDespesa carregar(int id);

		IQueryable<ModoPagamentoDespesa> listar(bool? ativo);
		
	}
}
