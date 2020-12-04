using System;
using System.Linq;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.Financeiro {

    public class TituloReceitaReciboVWBL : DefaultBL, ITituloReceitaReciboVWBL {


        //Carregamento do registro
        public TituloReceitaReciboVW carregar(int id) {

            var query = db.TituloReceitaReciboVW.Select(x => x);

            query = query.condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);
        }


        //Listagem de opcoes de pagamento realizadas
        public IQueryable<TituloReceitaReciboVW> listar() {

            var query = from Tit in this.db.TituloReceitaReciboVW
                        select Tit;

            query = query.condicoesSeguranca();

            return query;
        }
    }
}
