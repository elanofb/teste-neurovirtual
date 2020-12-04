using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels{
    
    public class PedidoDetalhesVM {
        
        public Pedido Pedido { get; set; }
 
        public PedidoEntrega PedidoEntrega { get; set; }
               
	}

}