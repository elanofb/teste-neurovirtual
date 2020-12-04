using System.Collections.Generic;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface INotificacaoSistemaEnvioCobrancaBL {

        void enviarEmail(List<NotificacaoSistemaEnvio> listaEnvios);
        
    }

}
