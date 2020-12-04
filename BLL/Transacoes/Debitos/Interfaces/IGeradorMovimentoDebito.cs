using System;
using DAL.Transacoes;

namespace BLL.Transacoes.Debitos {

    public interface IGeradorMovimentoDebito {
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno debitar(MovimentoResumoVW MovimentoResumo);
    }

}