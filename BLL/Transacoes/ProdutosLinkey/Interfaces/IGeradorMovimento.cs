using System;
using System.Collections.Generic;
using DAL.Pedidos;
using DAL.Transacoes;

namespace BLL.Transacoes.ProdutosLinkey {

    public interface IGeradorMovimento {
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno transferir(List<PedidoProdutoRendimento> listaProdutos);
    }

}