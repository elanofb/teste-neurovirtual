using System;
using System.Collections.Generic;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface INotificador {

        UtilRetorno notificar(List<NotificacaoSistemaEnvio> listaEnvios);
    }

}
