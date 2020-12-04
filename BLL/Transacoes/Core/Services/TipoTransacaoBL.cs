using System;
using System.Linq;
using BLL.Services;
using DAL.Transacoes;

namespace BLL.Transacoes {

	public class TipoTransacaoConsultaBL : DefaultBL, ITipoTransacaoConsultaBL {

		//
		public TipoTransacaoConsultaBL() {
		}

	    //
	    public IQueryable<TipoTransacao> query() {

	        var query = from Obj in db.TipoTransacao
	                    select Obj;
            
	        return query;

	    }

		//Carregamento de registro pelo ID
		public TipoTransacao carregar(int id) { 

		    var query = this.query().condicoesSeguranca();
            
		    return query.FirstOrDefault(x => x.id == id);

		}


	}
}