using System.Linq;
using DAL.Pedidos;

namespace BLL.Pedidos {

	public interface IPedidoEntregaBL {

		bool salvar(PedidoEntrega OPedidoEntrega);

		IQueryable<PedidoEntrega> listar();
	}
}
