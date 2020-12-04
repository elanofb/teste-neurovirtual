using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public class PedidoAcaoExpedicaoBL : DefaultBL, IPedidoAcaoExpedicaoBL {
        

        //Events
        private EventAggregator onPedidoExpedido => OnPedidoExpedido.getInstance;

		//
		public PedidoAcaoExpedicaoBL() {
            
        }

        //
        public void expedir(int idPedido, string observacoes) {
            
            db.Pedido.condicoesSeguranca().Where(x => x.id == idPedido)
              .Update(x => new Pedido {

                    dtExpedicao = DateTime.Now,

                    idStatusPedido = StatusPedidoConst.EXPEDIDO

              });
            
            var listaParametros = new List<object>();
            listaParametros.Add(idPedido);
            listaParametros.Add(observacoes);
            
            this.onPedidoExpedido.subscribe(new OnPedidoExpedidoHandler());
            this.onPedidoExpedido.publish(listaParametros as object);


        }
           
	}

}