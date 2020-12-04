using BLL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Pedidos {

    public class OnPedidoEmMontagemHandler : IHandler<object> {
        
        //Propriedades
        private IPedidoHistoricoBL OPedidoOcorrenciaBL => new PedidoHistoricoBL();
        
		//
		public void execute(object source) {
            
            try {
                
                var listaParametros = source as List<int>;

                var idsPedidos = listaParametros;            		        
                
                this.registrarOcorrencia(idsPedidos, "Pedido em processo de montagem");

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro ao executar o evento OnPedidoEmMontagemHandler.");

            }

		}
        
        // Gerar ocorrência de alteração de status do pedido
        private void registrarOcorrencia(List<int> idsPedidos, string observacoes) {
            
            foreach (int idPedido in idsPedidos){
                this.OPedidoOcorrenciaBL.criarOcorrenciaEmMontagem(idPedido, observacoes);    
            }            

        }
        
	}
}
