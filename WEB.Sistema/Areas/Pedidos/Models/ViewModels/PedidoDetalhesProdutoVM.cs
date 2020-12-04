using System.Collections.Generic;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels{
    
    public class PedidoDetalhesProdutoVM {
        
        public int idPedido { get; set; }

        public List<PedidoProduto> listaProdutos { get; set; }

	}

}