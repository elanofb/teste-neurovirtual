using System;
using System.Linq;
using BLL.Services;
using DAL.Transacoes;
using EntityFramework.Extensions;

namespace BLL.Transacoes {

    public class MovimentoCadastroBL : DefaultBL, IMovimentoCadastroBL {

        /// <summary>
        /// 
        /// </summary>
        public void atualizarSincronizacao(int[] idMembros, DateTime dtSincronizacao) {

            db.Movimento.Where(x => idMembros.Contains((int)x.idMembroDestino) && 
                                    x.dtIntegracaoSaldo == null)
              .Update(x => new Movimento {
                                             dtIntegracaoSaldo = dtSincronizacao
                                         });
            
        }
    }

}
