using System;
using System.Collections.Generic;

namespace BLL.AssociadosOperacoes {

    public interface IAssociadoAdmissaoAlteracaoBL {
        
        //
        UtilRetorno alterarAdmissao(List<int> idsAssociados, DateTime dtAdmissao, string observacoes);

    }

}
