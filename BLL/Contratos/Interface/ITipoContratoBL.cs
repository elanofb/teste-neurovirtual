using System.Linq;
using DAL.Contratos;

namespace BLL.Contratos {

    public interface ITipoContratoBL {

        IQueryable<TipoContrato> listar(string valorBusca, bool? ativo);

    }
}
