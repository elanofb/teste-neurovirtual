using System;

namespace BLL.AssociadosContribuicoes {
    public interface IAssociadoAlteracaoBL {
        UtilRetorno alterarDados(int idAssociado, string informacao, object novoValor, int idUsuarioOperacao);
    }
}