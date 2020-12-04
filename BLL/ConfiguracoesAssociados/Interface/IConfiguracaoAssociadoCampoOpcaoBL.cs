using System;
using System.Linq;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados
{
    public interface IConfiguracaoAssociadoCampoOpcaoBL {
        //
        ConfiguracaoAssociadoCampoOpcao carregar(int id, int? idOrganizacaoInf = null);

        IQueryable<ConfiguracaoAssociadoCampoOpcao> listar(int idCampo, int? idOrganizacaoInf = null);

        //
        bool salvar(ConfiguracaoAssociadoCampoOpcao OConfiguracao);

        bool clonarOpcoesCampo(int idCampoClone, int idCampo);

        //
        UtilRetorno excluir(int id);
    }
}