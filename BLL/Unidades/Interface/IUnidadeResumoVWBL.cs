using System.Linq;
using DAL.Unidades;

namespace BLL.Unidades {
    public interface IUnidadeResumoVWBL {
        IQueryable<UnidadeResumoVW> listar(string ativo, bool flagTodasUnidades = true);
    }
}