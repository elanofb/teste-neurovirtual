using System;
using System.Linq;
using DAL.Contribuicoes;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Contribuicoes {

	public class TipoContribuicaoBL : TableRepository<TipoContribuicao>, ITipoContribuicaoBL {

		//Carregamento de registro único pelo ID
		public TipoContribuicao carregar(int id) {
			var db = this.getDataContext();
			var query = from Tipo in db.TipoContribuicao
						where 
							Tipo.id == id && 
							Tipo.flagExcluido == "N"
						select Tipo;

			return query.FirstOrDefault();
		}

		//Listagem de Registros
		public IQueryable<TipoContribuicao> listar(string ativo) {
			var db = this.getDataContext();
			var query = from Tipo in db.TipoContribuicao.AsNoTracking()
						where 
							Tipo.flagExcluido == "N"
						select Tipo;

			if (!String.IsNullOrEmpty(ativo)) { 
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(TipoContribuicao OTipoContribuicao) {
			this.save(OTipoContribuicao);
			return (OTipoContribuicao.id > 0);
		}

	}
}