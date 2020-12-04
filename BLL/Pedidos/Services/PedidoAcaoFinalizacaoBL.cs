using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public class PedidoAcaoFinalizacaoBL : DefaultBL, IPedidoAcaoFinalizacaoBL {
        

        //Events
        private EventAggregator onPedidoFinalizado => OnPedidoFinalizado.getInstance;

		//
		public PedidoAcaoFinalizacaoBL() {
            
        }

        //
        public void finalizar(int idPedido, string observacoes) {
            
            db.Pedido.condicoesSeguranca().Where(x => x.id == idPedido)
              .Update(x => new Pedido {

                    dtFinalizado = DateTime.Now,

                    idStatusPedido = StatusPedidoConst.FINALIZADO

              });
            
            var listaParametros = new List<object>();
            listaParametros.Add(idPedido);
            listaParametros.Add(observacoes);

            this.onPedidoFinalizado.subscribe(new OnPedidoFinalizadoHandler());
            this.onPedidoFinalizado.publish(listaParametros as object);


        }
           
	}

}