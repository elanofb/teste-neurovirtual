using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public class PedidoCancelamentoBL : DefaultBL, IPedidoCancelamentoBL {
        
        //Events
        private EventAggregator onPedidoCancelado => OnPedidoCancelado.getInstance;

		//
		public PedidoCancelamentoBL() {
            
        }

        //Avançar situacao dos pedidos
        public void cancelar(int[] ids, string observacoes) {
            
            db.Pedido.condicoesSeguranca().Where(x => ids.Contains(x.id))
              .Update(x => new Pedido {

                dtCancelamento = DateTime.Now,

                idStatusPedido = StatusPedidoConst.CANCELADO

            });

            foreach (var idPedido in ids) {
                
                var listaParams = new List<object>();

                listaParams.Add(idPedido);

                listaParams.Add(observacoes);

                this.onPedidoCancelado.subscribe(new OnPedidoCanceladoHandler());

                this.onPedidoCancelado.publish(listaParams as object);   
                
            }

        }
           
	}

}