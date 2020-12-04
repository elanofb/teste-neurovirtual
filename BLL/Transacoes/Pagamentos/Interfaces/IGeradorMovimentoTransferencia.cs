using System;
using DAL.Transacoes;

namespace BLL.Transacoes.Pagamentos {

    public interface IGeradorMovimentoPagamento {
        
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno pagar(MovimentoResumoVW MovimentoResumo, MovimentoOperacaoDTO Transacao);
    }

}