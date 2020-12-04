using DAL.Financeiro;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Financeiro {

	public interface ICentroCustoMacroContaBL {

	    IQueryable<CentroCustoMacroConta> listar(int idMacroConta, int idCentroCusto);

	    void salvar(int idMacroConta, int idCentroCusto, List<CentroCustoMacroConta> listaCentroCusto);
	}
}
