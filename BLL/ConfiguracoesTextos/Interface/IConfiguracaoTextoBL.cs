using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Cargos;
using DAL.ConfiguracoesTextos;

namespace BLL.ConfiguracoesTextos {

    public interface IConfiguracaoTextoBL {

        IQueryable<ConfiguracaoTexto> query(int? idOrganizacaoParam = null);
        
        ConfiguracaoTexto carregar(int id, int idOrganizacaoParam = 0, bool flagCache = true);

        IQueryable<ConfiguracaoTexto> listar(string valorBusca, int? idOrganizacaoParam = null);

        List<ConfiguracaoTexto> listarFromCache(int idOrganizacaoParam = 0, bool flagCache = true);

        bool salvar(ConfiguracaoTexto OConfiguracoes);

    }
}
