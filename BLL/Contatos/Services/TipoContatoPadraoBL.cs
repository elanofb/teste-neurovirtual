using DAL.Contatos;
using DAL.Repository.Base;
using System;
using System.Linq;
using BLL.Services;

namespace BLL.Contatos {

	public class TipoContatoPadraoBL : DefaultBL, ITipoContatoPadraoBL {

		//
		public TipoContatoPadraoBL() {

		}

        //
        //Listagem de Registros
		public IQueryable<TipoContatoPadrao> listar(string ativo) {
			
			var query = from Tipo in db.TipoContatoPadrao.AsNoTracking()
						where 
							Tipo.flagExcluido == "N"
						select Tipo;

			if (!String.IsNullOrEmpty(ativo)) { 
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

	}
}