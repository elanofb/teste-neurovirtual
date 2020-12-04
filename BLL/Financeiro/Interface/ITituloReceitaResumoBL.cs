using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro
{
    public interface ITituloReceitaResumoBL
    {
        TituloReceitaResumoVW carregar(int id);
        IQueryable<TituloReceitaResumoVW> listar(string valorBusca, bool incluirExcluidos = false);
    }
}