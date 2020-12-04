using System;

namespace BLL.Pedidos {

	public interface IPedidoCobrancaBL {

		UtilRetorno enviarEmailCobranca(int idPedido);

	}
}
