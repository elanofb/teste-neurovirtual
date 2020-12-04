using System;

namespace BLL.AssociadosContribuicoes {

    public interface IAssociadoContribuicaoEmailBL {

        UtilRetorno enviarEmailCobranca(int idAssociadoContribuicao);

    }
}