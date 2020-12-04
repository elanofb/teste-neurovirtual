using System;

using DAL.Associados;

namespace BLL.Associados.Interface {

    public interface IAssociadoAprovacaoDocumentosBL {
        UtilRetorno aprovacaoDocumentos(int idAssociado);
    }

}