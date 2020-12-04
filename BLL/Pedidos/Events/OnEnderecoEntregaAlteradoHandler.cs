using BLL.Core.Events;
using System;

namespace BLL.Pedidos {

	public class OnEnderecoEntregaAlteradoHandler : IHandler<object> {

        //Propriedades
        private IPedidoFreteBL OPedidoFreteBL => new PedidoFreteBL();

		//
		public void execute(object source) {

            var idPedido = source.toInt();

            this.recalcularFrete(idPedido);
            
		}


        // Recalcular frete após alterar endereço de entrega
	    private void recalcularFrete(int idPedido) {

	        try {

    			this.OPedidoFreteBL.recalcularFrete(idPedido);

	        } catch (Exception ex) {

	            UtilLog.saveError(ex, $"OnEnderecoEntregaAlteradoHandler: Erro ao recalcular o frete do pedido { idPedido }");

	        }
	        
	    }
        
	}

}
