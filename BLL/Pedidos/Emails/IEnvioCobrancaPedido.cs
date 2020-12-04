using System;
using DAL.Pedidos;

namespace BLL.Pedidos.Emails {

	public interface IEnvioCobrancaPedido {

		UtilRetorno enviar(Pedido OPedido);

	}
}