using System;
using System.Json;
using System.Linq;
using DAL.FinanceiroLancamentos;

namespace BLL.FinanceiroLancamentos {

	public interface ICredorVWBL {

        CredorVW carregar(int id);

		IQueryable<CredorVW> listar(string valorBusca);
	}
}
