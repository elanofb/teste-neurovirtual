using System.Linq;
using BLL.Services;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public class TipoGeracaoContribuicaoBL : DefaultBL, ITipoGeracaoContribuicaoBL {

		//Carregamento de registro único pelo ID
		public TipoGeracaoContribuicao carregar(int id) {

            var query = from Tipo in db.TipoGeracaoContribuicao
						where 
							Tipo.id == id && 
							Tipo.flagExcluido == false
						select Tipo;

			return query.FirstOrDefault();
		}

		//Listagem de Registros
		public IQueryable<TipoGeracaoContribuicao> listar(bool? ativo) {
			
			var query = from Tipo in db.TipoGeracaoContribuicao.AsNoTracking()
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