using System;
using DAL.Transacoes;

namespace BLL.Transacoes.Compras {

    public interface IGeradorMovimentoCompra {
        
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno pagar(MovimentoResumoVW MovimentoResumo, MovimentoOperacaoDTO Transacao);
    }

}