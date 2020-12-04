using System;
using System.Collections.Generic;

namespace BLL.AssociadosOperacoes {

    public interface IAssociadoExclusaoBL {
        
        //Excluir um associado
        UtilRetorno excluirAssociados(List<int> idsAssociados, int idMotivoDesligamento, string observacoes);

    }

}
