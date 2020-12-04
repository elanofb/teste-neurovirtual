using System;
using System.Json;
using System.Linq;
using DAL.FinanceiroLancamentos;

namespace BLL.FinanceiroLancamentos {

	public interface ICredorBL {
		
		Credor carregar(int id);

		IQueryable<Credor> listar(string valorBusca, bool? ativo);

		object autocompletar(string term, int idCredor);

		bool salvar(Credor OCredor);

		bool existe(Credor OCredor);

        JsonMessageStatus alterarStatus(int id);

		UtilRetorno excluir(int id);
	}
}
