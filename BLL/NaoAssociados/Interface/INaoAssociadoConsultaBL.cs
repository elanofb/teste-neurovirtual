using System.Linq;
using DAL.Associados;

namespace BLL.NaoAssociados {
    public interface INaoAssociadoConsultaBL {

        Associado carregar(int idOrganizacaoParam, int idAssociado);

        IQueryable<Associado> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo, int? idOrganizacaoInf = null);

        IQueryable<Associado> query(int idOrganizacaoParam);
    }
}