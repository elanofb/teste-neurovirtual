using System;
using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public interface IContribuicaoTabelaPrecoBL {

		ContribuicaoTabelaPreco carregar(int id);

        IQueryable<ContribuicaoTabelaPreco> listar(int idContribuicao, bool? ativo);

        bool salvar(ContribuicaoTabelaPreco OContribuicaoTabelaPreco);

        UtilRetorno excluir(int id, int idUsuario);

	}
}