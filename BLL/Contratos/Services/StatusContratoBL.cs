using System;
using System.Linq;
using DAL.Contratos;
using DAL.Repository.Base;

namespace BLL.Contratos {

	public class StatusContratoBL : TableRepository<StatusContrato>, IStatusContratoBL {

		//
		public StatusContratoBL() {
		}

		//
		public IQueryable<StatusContrato> listar(string valorBusca, string ativo) {
			var db = this.getDataContext();
			var query = from T in db.StatusContrato
						where T.flagExcluido == "N"
						select T;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}
	}
}