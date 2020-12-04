using System;
using DAL.Notificacoes;

namespace BLL.Notificacoes.Interface {

    public interface IMensageiroNotificacao{

        UtilRetorno enviar(NotificacaoSistema Notificacao);
    }
}
