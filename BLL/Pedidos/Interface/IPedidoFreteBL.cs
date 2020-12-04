using System.Threading.Tasks;

namespace BLL.Pedidos {

    public interface IPedidoFreteBL {
        
        bool recalcularFrete(int idPedido);

	}

}