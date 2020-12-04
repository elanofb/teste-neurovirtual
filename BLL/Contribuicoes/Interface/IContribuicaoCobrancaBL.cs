using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {
    public interface IContribuicaoCobrancaBL {
        ContribuicaoCobranca carregar(int id);
        IQueryable<ContribuicaoCobranca> listar(int idContribuicao);
        bool salvar(ContribuicaoCobranca OContribuicaoCobranca);
    }
}