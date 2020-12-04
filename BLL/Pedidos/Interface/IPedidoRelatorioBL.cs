using System.Linq;
using DAL.Pedidos;
using DAL.Pedidos.DTO;

namespace BLL.Pedidos {

	public interface IPedidoRelatorioBL {

	    IQueryable<PedidoGeralDTO> query(int? idOrganizacaoParam = null);

		PedidoGeralDTO carregar(int id);

        IQueryable<PedidoGeralDTO> listar(string valorBusca, string ativo, int idStatusPedido);


	}
}
