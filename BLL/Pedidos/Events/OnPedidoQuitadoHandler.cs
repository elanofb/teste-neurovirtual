using BLL.Core.Events;
using System;

namespace BLL.Pedidos {

	public class OnPedidoQuitadoHandler : IHandler<object> {

		//Atributos

        //Propriedades

		//
		public void execute(object source) {

            int idPedido = (int)source;
            
            this.gerarOcorrencia(idPedido);

		}


        //gerar a ocorrencia apos criacao do pedido
	    private void gerarOcorrencia(int idPedido) {

	        try {

    			//this.OPedidoOperacaoBL.alterarStatus(idPedido, StatusPedidoConst.PAGO, "Pedido pago");

	        } catch (Exception ex) {

	            UtilLog.saveError(ex, $"Erro ao salvar ocorrencia do pedido { idPedido }");

	        }
	        
	    }


	}
}