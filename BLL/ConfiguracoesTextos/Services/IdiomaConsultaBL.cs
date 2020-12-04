using System;
using System.Linq;
using BLL.Services;
using DAL.ConfiguracoesTextos;

namespace BLL.ConfiguracoesTextos {

	public class IdiomaConsultaBL : DefaultBL, IIdiomaConsultaBL {

		//
		public IdiomaConsultaBL() {

		}

        //
		public IQueryable<Idioma> query(int? idOrganizacaoParam = null) {
			
			var query = from Obj in db.Idioma
						where 
							!Obj.dtExclusao.HasValue
						select Obj;

			if (idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}
			
			if (idOrganizacao > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
			}

			return query;
		}
		
		//
		public Idioma carregar(int id) {

			var query = this.query().condicoesSeguranca();

			return query.FirstOrDefault(x => x.id == id);
			
		}
		
		//
		public IQueryable<Idioma> listar(string valorBusca, bool? ativo = true) {
			
			var query = this.query().condicoesSeguranca();

			if (!valorBusca.isEmpty()) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;

		}

	}
}