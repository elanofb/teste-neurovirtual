using System.Linq;
using DAL.Associados;
using BLL.Services;

namespace BLL.Associados {

	public class MembroSaldoVwBL : DefaultBL, IMembroSaldoVwBL {
		
		/// <summary>
		/// 
		/// </summary>
		public IQueryable<MembroSaldoVW> query(int idMembro) {

			var query = from TA in db.MembroSaldoVW
				select TA;
            

			if (idMembro > 0) {
				query = query.Where(x => x.idMembro == idMembro);
			}

			return query;

		}

	}
}