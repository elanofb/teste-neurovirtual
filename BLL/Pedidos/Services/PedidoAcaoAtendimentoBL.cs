using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public class PedidoAcaoAtendimentoBL : DefaultBL, IPedidoAcaoAtendimentoBL {
        
        //Events
        private EventAggregator onPedidoAtendido => OnPedidoAtendido.getInstance;

		//
		public PedidoAcaoAtendimentoBL() {
            
        }

        //
        public void atender(int idPedido, string observacoes) {
            
            db.Pedido.condicoesSeguranca().Where(x => x.id == idPedido)
              .Update(x => new Pedido {

                    dtAtendimento = DateTime.Now,

                    idStatusPedido = StatusPedidoConst.ATENDIDO

              });
            
            var listaParametros = new List<object>();
            listaParametros.Add(idPedido);
            listaParametros.Add(observacoes);

            this.onPedidoAtendido.subscribe(new OnPedidoAtendidoHandler());
            this.onPedidoAtendido.publish(listaParametros as object);


        }
           
	}

}