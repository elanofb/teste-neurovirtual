using System;
using System.Collections.Generic;
using System.Linq;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados {

    public interface IConfiguracaoAssociadoCampoGrupoBL {

        /// <summary>
        /// Carregar as configurações de Associados PF da organização e cachear os dados se necessário
        /// </summary>
        ConfiguracaoAssociadoCampoGrupo carregar(int id, int? idOrganizacaoInf = null);

        IQueryable<ConfiguracaoAssociadoCampoGrupo> listar(int? idOrganizacaoInf = null);

        /// <summary>
        /// Carregar as configurações de campos por organização e colocar em cache (se houver)
        /// </summary>
        List<ConfiguracaoAssociadoCampoGrupo> listarFromCache(int idOrganizacaoParam, bool flagCache = true);

        /// <summary>
        /// Carregar as configurações de campos por organização e colocar em cache (se houver)
        /// </summary>
        List<ConfiguracaoAssociadoCampoGrupo> listarFromCacheOrDefault(int idOrganizacaoParam, bool flagCache = true);

        /// <summary>
        /// Carregar as configuracoes do arquivo padrao
        /// </summary>
        List<ConfiguracaoAssociadoCampoGrupo> listarFromDefault();

        /// <summary>
        /// Salvar um novo registro o atualizar
        /// </summary>
        bool salvar(ConfiguracaoAssociadoCampoGrupo OConfiguracao);

        /// <summary>
        /// Salvar um novo registro o atualizar
        /// </summary>
        UtilRetorno excluir(int id);

        /// <summary>
        /// Reordenar ordem de apresentação dos grupos de campos
        /// </summary>
        void reordenarExibicao(int idGrupo, byte ordem, int idTipoCampoCadastro);
    }
}