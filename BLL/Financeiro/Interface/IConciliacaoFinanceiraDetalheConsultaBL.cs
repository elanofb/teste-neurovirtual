using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro
{
    public interface IConciliacaoFinanceiraDetalheConsultaBL
    {
        IQueryable<ConciliacaoFinanceiraDetalhe> query(int? idOrganizacaoParam = null);
        
        ConciliacaoFinanceiraDetalhe carregar(int id);

        IQueryable<ConciliacaoFinanceiraDetalhe> listar(string valorBusca, bool? ativo = true);
    }
}