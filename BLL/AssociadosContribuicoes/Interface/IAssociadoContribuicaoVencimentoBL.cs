using DAL.Contribuicoes;

namespace BLL.AssociadosContribuicoes {
    public interface IAssociadoContribuicaoVencimentoBL {

        ContribuicaoVencimento retornarProximoVencimento(Contribuicao Contribuicao, int idAssociado);

    }
}