using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Cargos;
using DAL.ConfiguracoesTextos;

namespace BLL.ConfiguracoesTextos {

    public interface IConfiguracaoLabelBL {

        IQueryable<ConfiguracaoLabel> query(int? idOrganizacaoParam = null);
        
        ConfiguracaoLabel carregar(int id, int idOrganizacaoParam = 0, bool flagCache = true);

        IQueryable<ConfiguracaoLabel> listar(string valorBusca, int? idOrganizacaoParam = null);

        List<ConfiguracaoLabel> listarFromCache(int idOrganizacaoParam = 0, bool flagCache = true);

        bool salvar(ConfiguracaoLabel OConfiguracoes);
        
    }
}
