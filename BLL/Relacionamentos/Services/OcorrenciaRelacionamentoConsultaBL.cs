using System;
using System.Linq;
using BLL.Services;
using DAL.Relacionamentos;

namespace BLL.Relacionamentos {

	public class OcorrenciaRelacionamentoConsultaBL : DefaultBL, IOcorrenciaRelacionamentoConsultaBL {

		//
		public OcorrenciaRelacionamentoConsultaBL() {

		}

        //
		public IQueryable<OcorrenciaRelacionamento> query(int? idOrganizacaoParam = null) {
			
			var query = from Obj in db.OcorrenciaRelacionamento
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
		public OcorrenciaRelacionamento carregar(int id) {

			var query = this.query().condicoesSeguranca();

			return query.FirstOrDefault(x => x.id == id);
			
		}
		
		//
		public IQueryable<OcorrenciaRelacionamento> listar(string valorBusca, bool? ativo = true) {
			
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