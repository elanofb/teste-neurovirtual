using System.Linq;
using DAL.Organizacoes;

namespace BLL.Organizacoes {

    public interface IStatusOrganizacaoBL{
        StatusOrganizacao carregar(int id);
        IQueryable<StatusOrganizacao> listar(string valorBusca, bool? ativo);
    }
}
