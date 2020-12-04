using System;
using DAL.Transacoes;

namespace BLL.Transacoes.Transferencias {

    public interface IGeradorMovimentoTransferencia {
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno transferir(MovimentoResumoVW MovimentoResumo);
    }

}