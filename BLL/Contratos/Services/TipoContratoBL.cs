using System;
using System.Linq;
using BLL.Services;
using DAL.Contratos;
using DAL.Repository.Base;

namespace BLL.Contratos {

	public class TipoContratoBL : DefaultBL, ITipoContratoBL {

		//
		public TipoContratoBL() {
		}

		//
		public IQueryable<TipoContrato> listar(string valorBusca, bool? ativo) {

            var query = from T in db.TipoContrato
						where T.flagExcluido == false
						select T;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (ativo.HasValue) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}
	}
}