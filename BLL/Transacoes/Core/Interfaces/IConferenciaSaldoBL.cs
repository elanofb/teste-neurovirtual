using System.Linq;
using DAL.Transacoes;

namespace BLL.Transacoes {

    public interface IConferenciaSaldoBL {
        IQueryable<ConferenciaSaldoVW> query();
    }

}