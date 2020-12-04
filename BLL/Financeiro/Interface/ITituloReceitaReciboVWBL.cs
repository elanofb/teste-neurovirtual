using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro
{
    public interface ITituloReceitaReciboVWBL {
        TituloReceitaReciboVW carregar(int id);

        IQueryable<TituloReceitaReciboVW> listar();
    }
}