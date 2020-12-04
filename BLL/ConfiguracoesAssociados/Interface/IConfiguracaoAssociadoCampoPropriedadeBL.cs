using System;
using System.Linq;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados
{
    public interface IConfiguracaoAssociadoCampoPropriedadeBL {
        //
        ConfiguracaoAssociadoCampoPropriedade carregar(int id, int? idOrganizacaoInf = null);

        IQueryable<ConfiguracaoAssociadoCampoPropriedade> listar(int idCampo, int? idOrganizacaoInf = null);

        //
        bool salvar(ConfiguracaoAssociadoCampoPropriedade OConfiguracao);

        bool clonarPropriedadesCampo(int idCampoClone, int idCampo);

        //
        UtilRetorno excluir(int id);
    }
}