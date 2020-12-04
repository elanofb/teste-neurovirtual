using System.Linq;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados {

    public interface IFuncaoFiltroBL {
        IQueryable<FuncaoFiltro> listar(string valorBusca, bool? ativo);
    }
}