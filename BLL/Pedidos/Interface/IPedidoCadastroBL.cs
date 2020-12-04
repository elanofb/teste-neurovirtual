using DAL.Pedidos;
using DAL.PedidosTemp;

namespace BLL.Pedidos {

    public interface IPedidoCadastroBL {
        
        Pedido salvar(PedidoTemp OPedidoTemp);
        
	}

}