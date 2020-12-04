using System;
using System.Json;
using System.Linq;
using DAL.Diretorias;

namespace BLL.Diretorias {

    public interface IDiretoriaBL {

        IQueryable<Diretoria> query(int? idOrganizacaoParam = null);

        Diretoria carregar(int id);

        IQueryable<Diretoria> listar(string valorBusca, bool? ativo);

        bool existe(Diretoria ODiretoria, int id);

        bool salvar(Diretoria ODiretoria);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

    }
}
