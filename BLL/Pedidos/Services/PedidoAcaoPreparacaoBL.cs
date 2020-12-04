using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public class PedidoAcaoPreparacaoBL : DefaultBL, IPedidoAcaoPreparacaoBL {
        

        //Events
        private EventAggregator onPedidoPreparado => OnPedidoPreparado.getInstance;

		//
		public PedidoAcaoPreparacaoBL() {
            
        }

        //
        public void preparar(int idPedido, string observacoes) {
            
            db.Pedido.condicoesSeguranca().Where(x => x.id == idPedido)
              .Update(x => new Pedido {

                    dtPreparacao = DateTime.Now,

                    idStatusPedido = StatusPedidoConst.EM_PREPARACAO

              });
            
            var listaParametros = new List<object>();
            listaParametros.Add(idPedido);
            listaParametros.Add(observacoes);

            this.onPedidoPreparado.subscribe(new OnPedidoPreparadoHandler());
            this.onPedidoPreparado.publish(listaParametros as object);


        }
           
	}

}