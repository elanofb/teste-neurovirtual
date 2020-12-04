using System;
using DAL.Pedidos;

namespace BLL.Pedidos.Emails {

	public interface IEnvioFaturamentoPedido {

		UtilRetorno enviar(Pedido OPedido);

	}
}