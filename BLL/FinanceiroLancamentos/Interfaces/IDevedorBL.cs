using System;
using System.Json;
using System.Linq;
using DAL.FinanceiroLancamentos;

namespace BLL.FinanceiroLancamentos {

	public interface IDevedorBL {
		
		Devedor carregar(int id);

		IQueryable<Devedor> listar(string valorBusca, bool? ativo);

		object autocompletar(string term, int idDevedor);

		bool salvar(Devedor ODevedor);

		bool existe(Devedor ODevedor);

        JsonMessageStatus alterarStatus(int id);

		UtilRetorno excluir(int id);
	}
}
