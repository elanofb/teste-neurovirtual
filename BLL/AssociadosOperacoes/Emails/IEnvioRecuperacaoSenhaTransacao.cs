using System;
using DAL.Associados;
using DAL.Notificacoes;


namespace BLL.AssociadosOperacoes.Emails {

	public interface IEnvioRecuperacaoSenhaTransacao {

	    UtilRetorno enviar(NotificacaoSistemaEnvio OEnvio, string linkRecuperacao = "");

	}
}