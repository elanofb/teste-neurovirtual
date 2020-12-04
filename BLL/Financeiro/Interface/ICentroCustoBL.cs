using System.Linq;
using DAL.Financeiro;
using System.Json;
using System.Collections.Generic;

namespace BLL.Financeiro {

	public interface ICentroCustoBL {

        CentroCusto carregar(int id);

        IQueryable<CentroCusto> listar(string valorBusca, bool? ativo);

        bool existe(string descricao,int id);

        bool salvar(CentroCusto OTipoProduto);

        bool excluir(int id);

        JsonMessageStatus alterarStatus(int id);
	}
}
