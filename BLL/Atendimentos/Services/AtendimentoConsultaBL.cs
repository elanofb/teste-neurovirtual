using System.Linq;
using BLL.Services;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

	public class AtendimentoConsultaBL : DefaultBL, IAtendimentoConsultaBL {

        //
        public AtendimentoConsultaBL() {
		}

        //
        public Atendimento carregar(int id, int? idOrganizacaoParam = null) {

            var query = from Obj in db.Atendimento
						where Obj.id == id && Obj.flagExcluido == false
						select Obj;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query.FirstOrDefault();
        }


		//
		public IQueryable<Atendimento> listar(bool? ativo, int? idOrganizacaoParam = null) {
            
            var query = from Obj in db.Atendimento
						where Obj.flagExcluido == false
						select Obj;
            
            if (ativo.HasValue) {
				query = query.Where(x => x.ativo == ativo);
			}
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

			return query;
		}
        
	}
}