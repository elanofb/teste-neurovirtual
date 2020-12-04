using System;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes.Emails {

	public interface IEnvioPagamentoContribuicao {

		UtilRetorno enviar(AssociadoContribuicao OAssociadoContribuicao);

	}
}