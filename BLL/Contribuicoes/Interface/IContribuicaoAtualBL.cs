using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public interface IContribuicaoAtualBL {

		Contribuicao carregar();

		Contribuicao carregarAnuidade(short ano = 0);

        Contribuicao carregarMensalidade(int mes = 0, short ano = 0);
	}
}