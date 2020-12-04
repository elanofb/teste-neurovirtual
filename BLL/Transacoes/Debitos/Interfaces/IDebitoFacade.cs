using System;
using DAL.Transacoes;

namespace BLL.Transacoes.Debitos {

    public interface IDebitoFacade {
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno debitar(MovimentoOperacaoDTO Transacao);
    }

}