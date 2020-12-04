using System;
using System.Linq;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.Financeiro {

    public class ReceitaConsultaBL : DefaultBL, IReceitaConsultaBL {

        //Atributos

		//Propriedades

		//Listagem de pagamentos
	    public IQueryable<TituloReceitaPagamentoVW> listarPagamentos(int idTipoReceita, bool flagExcluido = false) {

		    var query = from Rec in db.TituloReceitaPagamentoVW
						select Rec;

	        query = query.condicoesSeguranca();


            if (!flagExcluido) {
                query = query.Where(x => x.dtExclusao == null);
            }

            if (idTipoReceita > 0) {
			    query = query.Where(x => x.idTipoReceita == idTipoReceita);
		    }

			return query;
	    }
    }
}