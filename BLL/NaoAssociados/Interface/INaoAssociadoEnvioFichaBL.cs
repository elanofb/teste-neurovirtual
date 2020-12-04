using System;

namespace BLL.NaoAssociados {

    public interface INaoAssociadoEnvioFichaBL {

        UtilRetorno enviarPorEmail(int idAssociado, string emails, int idUsuario);
        
    }
}