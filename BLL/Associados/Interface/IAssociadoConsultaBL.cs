using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

    public interface IAssociadoConsultaBL {

        IQueryable<Associado> query(int? idOrganizacaoParam = null);
        
        IQueryable<Associado> queryNoFilter(int? idOrganizacaoParam = null);

        Associado carregar(int id);

        IQueryable<Associado> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo);

    }
}