using System;
using System.Collections.Generic;
using DAL.Pedidos;

namespace BLL.Transacoes.ProdutosLinkey {

    public interface IPagadorLinkeyFacade {
        
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno pagar(List<PedidoProdutoRendimento> listaProdutos);
    }

}