using System;
using System.Linq;
using DAL.Pedidos;

namespace BLL.Pedidos {

	public interface IPedidoProdutoBL {
		
	    IQueryable<PedidoProduto> query();
        
	    PedidoProduto carregar(int id) ;
        
		IQueryable<PedidoProduto> listar(int idPedido);

		bool salvar(PedidoProduto OPedidoProduto);
		
	}

}