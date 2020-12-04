using System.Linq;
using BLL.Services;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public class TipoVencimentoBL : DefaultBL, ITipoVencimentoBL {

		//Carregamento de registro único pelo ID
		public TipoVencimento carregar(int id) {

            var query = from Tipo in db.TipoVencimento
						where 
							Tipo.id == id && 
							Tipo.flagExcluido == false
						select Tipo;

			return query.FirstOrDefault();
		}

		//Listagem de Registros
		public IQueryable<TipoVencimento> listar(bool? ativo) {
			
			var query = from Tipo in db.TipoVencimento.AsNoTracking()
						where 
							Tipo.flagExcluido == false
						select Tipo;

			if (ativo.HasValue) { 
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}


	}
}