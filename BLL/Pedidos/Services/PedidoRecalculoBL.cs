using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos.Extensions;

namespace BLL.Pedidos {

    public class PedidoRecalculoBL : DefaultBL, IPedidoRecalculoBL {
        
        //Atributos

        //Propriedades

        //Events

		//
		public PedidoRecalculoBL() {
            
        }
        
        //
        public void recalcularValorPedido(int idPedido) {

            var OPedido = db.Pedido.FirstOrDefault(x => x.id == idPedido);

            if (OPedido == null) {
                return;
            }

            var listaProdutos = OPedido.listaProdutos.Where(x => x.flagExcluido == "N").ToList();

            OPedido.valorProdutos = listaProdutos.getValorTotal();

            db.SaveChanges();

        }


	}

}