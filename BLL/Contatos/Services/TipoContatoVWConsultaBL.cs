using DAL.Contatos;
using System;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Contatos {

	public class TipoContatoVWConsultaBL : DefaultBL, ITipoContatoVWConsultaBL {

		//
		public TipoContatoVWConsultaBL() {

		}

        //
		public IQueryable<TipoContatoVW> query(int? idOrganizacaoParam = null) {
			
			var query = from Obj in db.TipoContatoVW
						select Obj;

			if (idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}
			
			if (idOrganizacao > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoParam || x.idOrganizacao == null);
			}

			return query;
		}
		
		//
		public TipoContatoVW carregar(int id) {

			var query = this.query(User.idOrganizacao());

			return query.FirstOrDefault(x => x.id == id);
			
		}
		
		//
		public IQueryable<TipoContatoVW> listar(string valorBusca, bool? ativo = true) {
			
			var query = this.query(User.idOrganizacao());

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