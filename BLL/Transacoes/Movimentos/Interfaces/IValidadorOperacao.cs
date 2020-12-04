using System;
using DAL.Transacoes;

namespace BLL.Transacoes.Movimentos {

    public interface IValidadorOperacao {
        
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno validar(MovimentoOperacaoDTO Transacao);

        /// <summary>
        /// 
        /// </summary>
        UtilRetorno validaSenha(MovimentoOperacaoDTO Transacao);
    }

}