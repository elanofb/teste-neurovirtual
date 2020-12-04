using System;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;

namespace BLL.Financeiro {

    public class ConciliacaoFinanceiraDetalheConsultaBL : DefaultBL, IConciliacaoFinanceiraDetalheConsultaBL {

        // 
        public IQueryable<ConciliacaoFinanceiraDetalhe> query(int? idOrganizacaoParam = null) {

            var query = from Obj in db.ConciliacaoFinanceiraDetalhe
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
        public ConciliacaoFinanceiraDetalhe carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);

        }

        //Listagem de registros de acordo com filtros
        public IQueryable<ConciliacaoFinanceiraDetalhe> listar(string valorBusca, bool? ativo = true) {

            var query = this.query().condicoesSeguranca();

            return query;
        }
    }
}
