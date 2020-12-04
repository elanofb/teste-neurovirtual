using System;
using DAL.Transacoes;

namespace BLL.Transacoes.Compras {

    public interface IValidadorPagamento{
        
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno validar(MovimentoOperacaoDTO Transacao);
    }

}