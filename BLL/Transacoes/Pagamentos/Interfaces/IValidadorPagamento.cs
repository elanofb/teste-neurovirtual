using System;
using DAL.Transacoes;

namespace BLL.Transacoes.Pagamentos {

    public interface IValidadorPagamento{
        
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno validar(MovimentoOperacaoDTO Transacao);
    }

}