using System.Linq;
using DAL.Pedidos;

namespace BLL.Pedidos {

	public interface IPedidoBL {

	    IQueryable<Pedido> query(int? idOrganizacaoParam = null);

        Pedido carregar(int id);

        IQueryable<Pedido> listar(string valorBusca, string ativo, int idStatusPedido);

        IQueryable<Pedido> listarPorPessoa(int idPessoa);

        bool salvar(Pedido OPedido);


	}
}
