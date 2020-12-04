using System;
using DAL.Pedidos;

namespace BLL.Pedidos {

	public interface IPedidoProdutoOperacaoBL {

        void adicionar(PedidoProduto OPedidoProduto);

	    UtilRetorno excluir(int id);

	}

}