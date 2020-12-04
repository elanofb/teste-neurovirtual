using System.Linq;
using DAL.Empresas;
using System.Json;
using System;

namespace BLL.Empresas
{
    public interface IEmpresaPorteBL
    {
        EmpresaPorte carregar(int id);

        IQueryable<EmpresaPorte> listar(int idOrganizacaoParam, string valorBusca, bool? ativo);

        bool salvar(EmpresaPorte OEmpresaPorte);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);
    }
}