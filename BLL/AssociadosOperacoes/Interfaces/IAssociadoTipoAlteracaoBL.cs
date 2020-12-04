using System;
using System.Collections.Generic;

namespace BLL.AssociadosOperacoes {

    public interface IAssociadoTipoAlteracaoBL {
        
        //
        UtilRetorno alterarTipo(List<int> idsAssociados, int dtAdmissao, string observacoes);

    }

}
