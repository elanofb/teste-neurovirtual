using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro {

	public interface IGatewayPagamentoBL {
        GatewayPagamento carregar(int id);
        IQueryable<GatewayPagamento> listar(string valorBusca,bool? ativo);
        bool existe(string descricao,int id);
        bool salvar(GatewayPagamento OGatewayPagamento);
        bool excluir(int id);
	}
}
