using System;
using System.Linq;
using DAL.Permissao;
using System.Json;

namespace BLL.Permissao {

    public interface IPerfilAcessoBL {

        PerfilAcesso carregar(int id);

        IQueryable<PerfilAcesso> listar(int idOrganizacao, string valorBusca, string ativo);

        bool salvar(PerfilAcesso OPerfilAcesso);

        object getAutoComplete(string term);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

        JsonMessageStatus alterarStatus(int id);
    }
}
