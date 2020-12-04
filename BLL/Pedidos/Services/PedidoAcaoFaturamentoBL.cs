using System;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public class PedidoAcaoFaturamentoBL : DefaultBL, IPedidoAcaoFaturamentoBL {
        
        //Atributos
        private IPedidoHistoricoBL _IPedidoHistoricoBL;

        //Propriedades
        private IPedidoHistoricoBL OPedidoOcorrenciaBL => _IPedidoHistoricoBL = _IPedidoHistoricoBL ?? new PedidoHistoricoBL();

        //Events
        private EventAggregator onPedidoFaturado => OnPedidoFaturado.getInstance;

		//
		public PedidoAcaoFaturamentoBL() {
            
        }

        // 
        public void salvarDadosFaturamento(Pedido OPedido) {

            db.Pedido.condicoesSeguranca().Where(x => x.id == OPedido.id)
              .Update(x => new Pedido {

                    dtVencimento = OPedido.dtVencimento,

                    idContaBancaria = OPedido.idContaBancaria,

                    codigoContabil = OPedido.codigoContabil,

                    idCentroCusto = OPedido.idCentroCusto,
                                
                    idMacroConta = OPedido.idMacroConta,

                    idCategoriaTitulo = OPedido.idCategoriaTitulo,

                    flagCartaoCreditoPermitido = OPedido.flagCartaoCreditoPermitido,

                    flagBoletoBancarioPermitido = OPedido.flagBoletoBancarioPermitido,

                    flagDepositoPermitido = OPedido.flagDepositoPermitido

              });
            
            this.faturarPedido(OPedido.id);

        }

        //
        public void faturarPedido(int idPedido) {

            var OPedido = db.Pedido.FirstOrDefault(x => x.id == idPedido);

            if (OPedido == null) {

                return;
            }

            OPedido.dtFaturamento = DateTime.Now;

            OPedido.idStatusPedido = StatusPedidoConst.AGUARDANDO_PAGAMENTO;

            db.SaveChanges();
            
            //
            this.onPedidoFaturado.subscribe(new OnPedidoFaturadoHandler());

            this.onPedidoFaturado.publish(OPedido as object);

        }
        
	}

}