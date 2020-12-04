using System;
using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro
{
    public interface IConciliacaoFinanceiraConsultaBL
    {
        IQueryable<ConciliacaoFinanceira> query(int? idOrganizacaoParam = null);
        
        ConciliacaoFinanceira carregar(int id);

        IQueryable<ConciliacaoFinanceira> listar(DateTime? dtConciliacao, int idUsuarioConciliacao);
    }
}