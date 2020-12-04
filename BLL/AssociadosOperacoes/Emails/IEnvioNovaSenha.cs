using System;
using DAL.Associados;
using DAL.Notificacoes;

namespace BLL.AssociadosOperacoes.Emails {

	public interface IEnvioNovaSenha {

		UtilRetorno enviar(Associado OAssociado, NotificacaoSistema ONotificacao);
        
	}

}