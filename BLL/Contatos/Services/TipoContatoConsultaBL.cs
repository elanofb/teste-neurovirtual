using DAL.Contatos;
using System;
using System.Linq;
using BLL.Services;

namespace BLL.Contatos {

	public class TipoContatoConsultaBL : DefaultBL, ITipoContatoConsultaBL {

		//
		public TipoContatoConsultaBL() {

		}

        //
		public IQueryable<TipoContato> query(int? idOrganizacaoParam = null) {
			
			var query = from Obj in db.TipoContato
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
		public TipoContato carregar(int id) {

			var query = this.query().condicoesSeguranca();

			return query.FirstOrDefault(x => x.id == id);
			
		}
		
		//
		public IQueryable<TipoContato> listar(string valorBusca, bool? ativo = true) {
			
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