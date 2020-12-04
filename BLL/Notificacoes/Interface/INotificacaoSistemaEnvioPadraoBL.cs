using System;
using System.Collections.Generic;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface INotificacaoSistemaEnvioPadraoBL {
        
		void enviarEmail(List<NotificacaoSistemaEnvio> listaEnvios);

		UtilRetorno enviarEmail(NotificacaoSistemaEnvio oNotificacaoSistemaEnvio, List<string> listaEmails);

    }

}
