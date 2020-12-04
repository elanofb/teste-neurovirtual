using System;
using DAL.Transacoes;

namespace BLL.Transacoes.Pagamentos {

    public interface IPagamentoFacade {
        
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno pagar(MovimentoOperacaoDTO Transacao);
    }

}