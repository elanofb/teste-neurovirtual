using System;
using System.Linq;
using DAL.Instituicoes;

namespace BLL.Instituicoes {

    public interface IInstituicaoBL {

        Instituicao carregar(int id);

        IQueryable<Instituicao> listar(int idOrganizacaoParam, string valorBusca, bool? ativo);

        bool existe(string descricao, int idOrganizacao, int id);

        bool salvar(Instituicao OInstituicao);

        UtilRetorno alterarStatus(int id);

        UtilRetorno excluir(int id);

    }
}
