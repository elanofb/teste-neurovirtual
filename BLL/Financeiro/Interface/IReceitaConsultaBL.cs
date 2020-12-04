using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro {

	public interface IReceitaConsultaBL {

		IQueryable<TituloReceitaPagamentoVW> listarPagamentos(int idTipoReceita, bool flagExcluido = false);
    
	}
}
