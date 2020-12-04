using System.Linq;
using DAL.Pedidos;

namespace BLL.Pedidos {
    public interface IStatusPedidoBL {

        IQueryable<StatusPedido> listar(string valorBusca, string ativo);

    }
}
