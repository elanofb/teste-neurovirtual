using System;
using DAL.Pedidos;

namespace BLL.Pedidos.Emails {

	public interface IEnvioNovoPedido {

		UtilRetorno enviar(Pedido OPedido);

	}
}