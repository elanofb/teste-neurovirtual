using System.Linq;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes
{
    public interface IAssociadoContribuicaoResumoBL
    {
        IQueryable<AssociadoContribuicaoResumoVW> listar(int idContribuicao, int idAssociado);
    }
}