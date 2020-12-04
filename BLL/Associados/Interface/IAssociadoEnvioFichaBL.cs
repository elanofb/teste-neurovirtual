using System;

namespace BLL.Associados {

    public interface IAssociadoEnvioFichaBL {

        UtilRetorno enviarPorEmail(int idAssociado, string emails, int idUsuario);
        
    }
}