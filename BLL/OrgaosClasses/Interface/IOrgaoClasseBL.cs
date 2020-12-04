using System;
using System.Json;
using System.Linq;
using DAL.OrgaosClasses;

namespace BLL.OrgaosClasses {

    public interface IOrgaoClasseBL {

        OrgaoClasse carregar(int id);

        IQueryable<OrgaoClasse> listar(string valorBusca, bool? ativo, int? idOrganizacaoInf = null);

        bool existe(OrgaoClasse OOrgaoClasse, int id);

        bool salvar(OrgaoClasse OOrgaoClasse);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

    }
}
