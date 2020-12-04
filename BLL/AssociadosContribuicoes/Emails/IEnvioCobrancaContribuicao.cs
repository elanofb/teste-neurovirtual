using System;
using DAL.AssociadosContribuicoes;
using DAL.Notificacoes;

namespace BLL.AssociadosContribuicoes.Emails {

	public interface IEnvioCobrancaContribuicao {

		UtilRetorno enviar(NotificacaoSistema ONotificacao, AssociadoContribuicaoResumoVW OAssociadoContribuicao);

	}
}