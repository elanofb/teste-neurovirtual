using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public class ConfiguracaoContribuicaoBL : DefaultBL, IConfiguracaoContribuicaoBL {

        //Constantes
        private static ConfiguracaoContribuicaoBL _instance;

        //Propriedades
        public static ConfiguracaoContribuicaoBL getInstance => _instance = _instance ?? new ConfiguracaoContribuicaoBL();

        /// <summary>
        /// Carregar as configurações de sistema da transportadora e cachear os dados se necessário
        /// </summary>
        public ConfiguracaoContribuicao carregar(int idOrganizacaoInf = 0, bool flagCache = true) {

            if (idOrganizacao > 0) {
                idOrganizacaoInf = idOrganizacao;
            }

            if (flagCache) {
                var cacheData = CacheService.getInstance.carregar(CacheService.CONFIGURACAO_CONTRIBUICAO);

                if (cacheData != null) {
                    return (ConfiguracaoContribuicao)cacheData;
                }
            }

            var query = db.ConfiguracaoContribuicao.AsNoTracking().Where(x => x.flagExcluido == false)
                .Include(x => x.ContaBancariaPadrao)
                .Include(x => x.CentroCustoPadrao)
                .Include(x => x.MacroContaPadrao);

            query = idOrganizacaoInf > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoInf) : query.Where(x => x.idOrganizacao == null);

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? new ConfiguracaoContribuicao();

            if (flagCache) {
                CacheService.getInstance.adicionar(CacheService.CONFIGURACAO_CONTRIBUICAO, OConfiguracao);
            }
            return OConfiguracao;
        }

        //Configurações gerais
        public IQueryable<ConfiguracaoContribuicao> listar(int idOrganizacaoInf) {

            if (idOrganizacao > 0) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = db.ConfiguracaoContribuicao.AsNoTracking()
                    .Where(x => x.flagExcluido == false)
                    .Include(x => x.Organizacao)
                    .Include(x => x.ContaBancariaPadrao)
                    .Include(x => x.CentroCustoPadrao)
                    .Include(x => x.MacroContaPadrao);

            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            return query;
        }

        /// <summary>
        /// Salvar configurações de serviços e remover  os registros anteriores.
        /// </summary>
        public bool salvar(ConfiguracaoContribuicao OConfiguracoes) {

            OConfiguracoes.id = 0;
            OConfiguracoes.setDefaultInsertValues();

            db.ConfiguracaoContribuicao.Add(OConfiguracoes);
            db.SaveChanges();

            bool flagSucesso = OConfiguracoes.id > 0;

            if (flagSucesso) {
                CacheService.getInstance.remover(CacheService.CONFIGURACAO_CONTRIBUICAO);
            }
            return flagSucesso;
        }
    }
}