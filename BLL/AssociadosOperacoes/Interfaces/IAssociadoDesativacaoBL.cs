using System;
using System.Collections.Generic;

namespace BLL.AssociadosOperacoes {

    public interface IAssociadoDesativacaoBL {

        UtilRetorno desativarAssociados(List<int> idsAssociados, int idMotivoDesativacao, string motivoDesativacao);

    }

}
