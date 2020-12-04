using DAL.Financeiro;
using System.Linq;
using BLL.Services;

namespace BLL.Financeiro {

    public interface IReceitasDespesasArquivosVWBL {

        IQueryable<ReceitaDespesaArquivoVW> listar();
        
    }

}
