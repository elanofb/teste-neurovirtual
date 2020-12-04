using System;
using System.Collections.Generic;
using System.Linq;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados {

    public interface IConfiguracaoAssociadoCampoTipoAssociadoBL {

        ConfiguracaoAssociadoCampoTipoAssociado carregar(int id, int? idOrganizacaoParam = null);

        IQueryable<ConfiguracaoAssociadoCampoTipoAssociado> listar(int? idOrganizacaoParam = null);

        bool salvar(ConfiguracaoAssociadoCampoTipoAssociado OConfiguracaoTipoAssociado);

        bool salvarEmLote(List<ConfiguracaoAssociadoCampoTipoAssociado> listaConfiguracaoTipoAssociado);

        UtilRetorno excluir(List<int> ids);

    }
}