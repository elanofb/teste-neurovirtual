using System;
using DAL.Notificacoes;

namespace BLL.AssociadosOperacoes.Emails {

	public interface IEnvioAtualizacaoCadastro {

		UtilRetorno enviar(NotificacaoSistemaEnvio OEnvio, NotificacaoSistema ONotificacao);

	}

}