using System;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;

namespace BLL.Financeiro {

    public class ConciliacaoFinanceiraConsultaBL : DefaultBL, IConciliacaoFinanceiraConsultaBL {

        // 
        public IQueryable<ConciliacaoFinanceira> query(int? idOrganizacaoParam = null) {

            var query = from Obj in db.ConciliacaoFinanceira
                where Obj.dtExclusao == null
                select Obj;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //Carregamento de registro pelo ID
        public ConciliacaoFinanceira carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);

        }

        //Listagem de registros de acordo com filtros
        public IQueryable<ConciliacaoFinanceira> listar(DateTime? dtConciliacao, int idUsuarioConciliacao) {

            var query = this.query().condicoesSeguranca();

            if (dtConciliacao.HasValue) {
                query = query.Where(x => x.dtConciliacao == dtConciliacao);
            }
            
            if (idUsuarioConciliacao > 0) {
                query = query.Where(x => x.idUsuarioCadastro == idUsuarioConciliacao);
            }

            return query;
        }
    }
}
