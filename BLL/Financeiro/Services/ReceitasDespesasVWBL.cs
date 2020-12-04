using System;
using DAL.Financeiro;
using System.Linq;
using BLL.Services;

namespace BLL.Financeiro {

    public class ReceitasDespesasVWBL : DefaultBL, IReceitasDespesasVWBL {

        public IQueryable<ReceitaDespesaVW> listar() {
            
            var query = from RD in db.ReceitaDespesaVW
                        select RD;

            query = query.condicoesSeguranca();

            return query;

        }


    }

}
