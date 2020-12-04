using System;
using System.Linq;
using BLL.Frete;
using BLL.Services;
using DAL.Frete;
using DAL.Pedidos;
using DAL.Pedidos.Extensions;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public class PedidoFreteBL : DefaultBL, IPedidoFreteBL {
        
        // Atributos
        private IPedidoBL _IPedidoBL;
		private FreteSyncBL _FreteSyncBL;

        // Propriedades
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();
        private FreteSyncBL OFreteSyncBL => _FreteSyncBL = _FreteSyncBL ?? new FreteSyncBL();

        //
        public bool recalcularFrete(int idPedido) {

            var OPedido = this.OPedidoBL.carregar(idPedido.toInt());

            if (OPedido == null) {
                return false;
            }

            var listaProdutos = OPedido.listaProdutos.Where(x => x.flagExcluido == "N" && x.flagCalcularFrete == true).ToList();

            var pesoTotal = listaProdutos.getPesoTotal();

            var OEnderecoEntrega = OPedido.listaPedidoEntrega.FirstOrDefault(x => x.flagExcluido == "N");

            if (OEnderecoEntrega == null) {

                this.registrarFrete(idPedido, null, 0);

                return false;

            }

            if (pesoTotal == 0) {

                this.registrarFrete(idPedido, null, 0);

                return false;
            }
            
            var ORetornoFrete = this.OFreteSyncBL.buscar(OEnderecoEntrega.cepOrigem, OEnderecoEntrega.cep, pesoTotal, 0, 0, 0);

            var OFrete = ORetornoFrete.FirstOrDefault(x => x.codigoServico == CorreiosTipoFreteConst.SEDEX);

            if (OFrete == null) {
                
                this.registrarFrete(idPedido, null, 0);

                return false;

            }

            this.registrarFrete(idPedido, OFrete.codigoServico, OFrete.valorEntrega);
            
            return true;

        }

        //
        private void registrarFrete(int idPedido, int? idTipoFrete, decimal valorFrete) {

            db.PedidoEntrega.Where(x => x.idPedido == idPedido && x.flagExcluido == "N")
              .Update(x => new PedidoEntrega { idTipoFrete = idTipoFrete });

            db.Pedido.Where(x => x.id == idPedido)
              .Update(x => new Pedido { valorFrete = valorFrete });

        }

	}

}