using System;
using DAL.Financeiro;
using System.Linq;
using BLL.Services;

namespace BLL.Financeiro {

    public class ReceitasDespesasArquivosVWBL : DefaultBL, IReceitasDespesasArquivosVWBL {

        public IQueryable<ReceitaDespesaArquivoVW> listar() {
            
            var query = from RD in db.ReceitaDespesaArquivoVW
                        select RD;

            query = query.condicoesSeguranca();

            return query;

        }


    }

}
