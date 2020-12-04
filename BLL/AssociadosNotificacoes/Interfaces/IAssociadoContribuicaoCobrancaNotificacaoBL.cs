using System.Collections.Generic;
using DAL.Contribuicoes;

namespace BLL.AssociadosNotificacoes.Services {

    public interface IAssociadoContribuicaoCobrancaNotificacaoBL {

        bool registrarEmailsCobrancas(Contribuicao OContribuicao, List<int> idsAssociados);

    }
    
}
