using System.Linq;
using BLL.Services;
using DAL.Transacoes;

namespace BLL.Transacoes {

	public class ConferenciaSaldoBL : DefaultBL, IConferenciaSaldoBL {

		//
		public ConferenciaSaldoBL() {
		}

	    //
	    public IQueryable<ConferenciaSaldoVW> query() {

	        var query = from Obj in db.ConferenciaSaldoVW
	                    select Obj;
            
	        return query;

	    }

		

	}
}