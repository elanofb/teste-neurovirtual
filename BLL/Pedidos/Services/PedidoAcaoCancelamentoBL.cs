using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public class PedidoAcaoCancelamentoBL : DefaultBL, IPedidoAcaoCancelamentoBL {
        
        //Atributos
        private IPedidoHistoricoBL _IPedidoHistoricoBL;

        //Propriedades
        private IPedidoHistoricoBL OPedidoOcorrenciaBL => _IPedidoHistoricoBL = _IPedidoHistoricoBL ?? new PedidoHistoricoBL();

        //Events
        private EventAggregator onPedidoCancelado => OnPedidoCancelado.getInstance;

		//
		public PedidoAcaoCancelamentoBL() {
            
        }

        //Avançar situacao dos pedidos
        public void cancelar(int idPedido, string observacoes) {
            
            db.Pedido.condicoesSeguranca().Where(x => x.id == idPedido)
              .Update(x => new Pedido {

                    dtCancelamento = DateTime.Now,

                    idStatusPedido = StatusPedidoConst.CANCELADO

              });

            this.OPedidoOcorrenciaBL.criarOcorrenciaCancelado(idPedido, observacoes);

            var listaParams = new List<object>();

            listaParams.Add(idPedido);

            listaParams.Add(observacoes);

            this.onPedidoCancelado.subscribe(new OnPedidoCanceladoHandler());

            this.onPedidoCancelado.publish(listaParams as object);


        }
           
	}

}