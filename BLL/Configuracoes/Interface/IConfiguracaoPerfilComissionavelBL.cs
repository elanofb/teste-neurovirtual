using System;
using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

    public interface IConfiguracaoPerfilComissionavelBL {

        ConfiguracaoPerfilComissionavel carregar(int id);

        IQueryable<ConfiguracaoPerfilComissionavel> listar(int idConfiguracaoComissao, int idPerfilAcesso);

        bool salvar(ConfiguracaoPerfilComissionavel OConfiguracaoPerfilComissionavel);

        UtilRetorno excluir(int id);
    }
}
