using System;
using BLL.Associados;
using DAL.Transacoes;

namespace BLL.Transacoes.Compras {

    public interface ICompraFacade {
        

        
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno pagar(MovimentoOperacaoDTO Transacao);
    }

}