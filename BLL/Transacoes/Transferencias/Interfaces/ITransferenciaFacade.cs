using System;
using DAL.Transacoes;

namespace BLL.Transacoes.Transferencias {

    public interface ITransferenciaFacade {
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno transferir(MovimentoOperacaoDTO Transacao);
    }

}