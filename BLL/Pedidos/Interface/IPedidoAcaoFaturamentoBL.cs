using DAL.Pedidos;

namespace BLL.Pedidos {

    public interface IPedidoAcaoFaturamentoBL {
        
        void salvarDadosFaturamento(Pedido OPedido);

        void faturarPedido(int idPedido);
        
	}

}