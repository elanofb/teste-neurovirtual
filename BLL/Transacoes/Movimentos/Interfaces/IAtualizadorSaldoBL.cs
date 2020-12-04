using System;
using System.Collections.Generic;
using DAL.Transacoes;

namespace BLL.Transacoes.Movimentos {

    public interface IAtualizadorSaldoBL {
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno atualizar(List<Movimento> listaMovimentos);
    }

}