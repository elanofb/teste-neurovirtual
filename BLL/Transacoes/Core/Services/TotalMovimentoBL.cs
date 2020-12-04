using System;
using System.Linq;
using BLL.Services;
using DAL.Transacoes;

namespace BLL.Transacoes {

	public class TotalMovimentoBL : DefaultBL, ITotalMovimentoBL {

		//
		public TotalMovimentoBL() {
		}

	    //
	    public IQueryable<TotalMovimentoVW> query() {

	        var query = from Obj in db.TotalMovimentoVW
	                    select Obj;
            
	        return query;

	    }



	}
}