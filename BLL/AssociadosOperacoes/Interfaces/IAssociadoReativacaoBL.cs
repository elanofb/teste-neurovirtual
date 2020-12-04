using System;
using System.Collections.Generic;

namespace BLL.AssociadosOperacoes {

    public interface IAssociadoReativacaoBL {
        
        UtilRetorno reativarAssociados(List<int> idsAssociados, string observacoes);

    }

}
