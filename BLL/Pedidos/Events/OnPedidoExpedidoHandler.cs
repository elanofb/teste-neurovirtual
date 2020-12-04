using BLL.Core.Events;
using System;
using System.Collections.Generic;

namespace BLL.Pedidos {

    public class OnPedidoExpedidoHandler : IHandler<object> {

        //Propriedades
        private IPedidoHistoricoBL OPedidoOcorrenciaBL => new PedidoHistoricoBL();

		//
		public void execute(object source) {

            try {

                var listaParametros = source as List<object>;

                var idPedido = Convert.ToInt32(listaParametros[0]);
            
		        var observacoes = listaParametros[1]?.ToString();
                
                this.registrarOcorrencia(idPedido, observacoes);

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro ao executar o evento OnPedidoExpedidoHandler.");

            }

		}
        
        // Gerar ocorrência de expedição do pedido
        private void registrarOcorrencia(int id, string observacoes) {

            this.OPedidoOcorrenciaBL.criarOcorrenciaExpedido(id, observacoes);

        }
        
	}
}
