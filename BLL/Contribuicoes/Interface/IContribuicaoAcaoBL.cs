using System;

namespace BLL.Contribuicoes {

	public interface IContribuicaoAcaoBL {

		UtilRetorno processar(int idContribuicao);

		UtilRetorno processar(int idContribuicao, int mes, int ano);

        UtilRetorno cancelar(int idContribuicao, string motivo);

        UtilRetorno iniciarCobranca(int idContribuicao);

        UtilRetorno iniciarCobranca(int idContribuicao, int mes, int ano);

        UtilRetorno enviarEmailCobranca(int idAssociadoContribuicao);
	}
}