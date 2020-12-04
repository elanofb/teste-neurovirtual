using System.Linq;
using DAL.Financeiro;
using System.Json;

namespace BLL.Financeiro {

	public interface ITipoReceitaBL {

        TipoReceita carregar(int id);

        IQueryable<TipoReceita> listar(string valorBusca, bool? ativo);

        bool existe(string descricao,int id);

        bool salvar(TipoReceita OTipoProduto);

        bool excluir(int id);

        JsonMessageStatus alterarStatus(int id);
	}
}
