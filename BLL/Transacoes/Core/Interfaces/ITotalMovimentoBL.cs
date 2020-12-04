using System.Linq;
using DAL.Transacoes;

namespace BLL.Transacoes {

    public interface ITotalMovimentoBL {
        IQueryable<TotalMovimentoVW> query();
    }

}