using System;
using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

    public interface IContribuicaoPrecoBL {

        ContribuicaoPreco carregar(int id);

        IQueryable<ContribuicaoPreco> listar(int idContribuicao, int idTabelaPreco, string ativo);

        bool salvar(ContribuicaoPreco OContribuicaoPreco);

        UtilRetorno excluir(int id, int idUsuarioExclusao);
    }
}