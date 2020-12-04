using System.Linq;
using DAL.Associados;
using BLL.Services;

namespace BLL.Associados {

	public class MembroSaldoConsultaBL : DefaultBL, IMembroSaldoConsultaBL {
		
		/// <summary>
		/// 
		/// </summary>
		public IQueryable<MembroSaldo> query(int idMembro, int? idOrganizacaoParam = null) {

			var query = from TA in db.MembroSaldo
				select TA;
            
			if (idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}

			if (idOrganizacaoParam > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
			}

			if (idMembro > 0) {
				query = query.Where(x => x.idMembro == idMembro);
			}
			return query;

		}

	}
}