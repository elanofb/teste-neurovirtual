using System.Linq;
using DAL.Transacoes;

namespace BLL.Transacoes {

    public interface ITipoTransacaoConsultaBL {
        
        IQueryable<TipoTransacao> query();
        
        TipoTransacao carregar(int id);
    }

}