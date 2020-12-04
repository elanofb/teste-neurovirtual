using System;
using DAL.Pedidos;

namespace BLL.RedeAfiliados.Services {

    public interface IGeradorPontuacaoRede {
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno gerarPontos( PedidoProduto ItemProduto, byte idChaveBinaria, int idMembroInicio);
    }

}