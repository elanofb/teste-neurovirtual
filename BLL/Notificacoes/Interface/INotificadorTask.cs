using System;

namespace BLL.Notificacoes {

    public interface INotificadorTask {

        UtilRetorno executar(int idOrganizacaoParam);
    }

}
