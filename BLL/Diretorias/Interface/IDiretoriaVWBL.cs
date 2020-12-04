using System.Linq;
using DAL.Diretorias;

namespace BLL.Diretorias {

    public interface IDiretoriaVWBL {

        DiretoriaVW carregar(int id);

        IQueryable<DiretoriaVW> listar(string valorBusca, bool? ativo);

    }
}
