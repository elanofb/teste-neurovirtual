using System.Collections.Generic;
using System.Linq;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados {

    public interface IConfiguracaoAssociadoCampoBL {
        /// <summary>
        /// Carregar as configurações de Associados PF da organização e cachear os dados se necessario
        /// </summary>
        ConfiguracaoAssociadoCampo carregar(int id, int? idOrganizacaoParam = null);


        IQueryable<ConfiguracaoAssociadoCampo> listar(int idGrupo, bool? ativo, int? idOrganizacaoParam = null);

        /// <summary>
        /// Carregar as configuracoes de campos por organizacao e colocar em cache (se houver)
        /// </summary>
        List<ConfiguracaoAssociadoCampo> listarFromCache(int idOrganizacaoParam, bool flagCache = true);

        /// <summary>
        /// Carregar as configuracoes de campos por organizacao e colocar em cache (se houver)
        /// </summary>
        List<ConfiguracaoAssociadoCampo> listarFromCacheOrDefault(int idOrganizacaoParam, bool flagCache = true);


        List<ConfiguracaoAssociadoCampo> listarFromDefault();

        /// <summary>
        /// Salvar um novo registro o atualizar
        /// </summary>
        bool salvar(ConfiguracaoAssociadoCampo OConfiguracao);
        
    }
}