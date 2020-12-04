using DAL.Financeiro;
using System.Linq;
using BLL.Services;

namespace BLL.Financeiro {

    public interface IReceitasDespesasVWBL {

        IQueryable<ReceitaDespesaVW> listar();
        
    }

}
