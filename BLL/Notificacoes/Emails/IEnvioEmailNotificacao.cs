using System;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

	public interface IEnvioEmailNotificacao {

		UtilRetorno enviar(NotificacaoSistemaEnvio oNotificacaoSistemaEnvio);

	}
}