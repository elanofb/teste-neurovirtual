using System.Collections.Generic;

namespace BLL.Pedidos {

    public interface IMovimentacaoPedidoBL {
        
        bool alterarStatus(List<int> idsPedidos, int idStatusPedido);
           
	}

}