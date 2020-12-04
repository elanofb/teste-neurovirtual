using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro {
    public interface ITituloReceitaConsultaBL {

        TituloReceita carregar(int id, bool? flagExcluido = false);

        IQueryable<TituloReceita> query(int? idOrganizacaoParam = null, bool? flagExcluido = false);
    }
}