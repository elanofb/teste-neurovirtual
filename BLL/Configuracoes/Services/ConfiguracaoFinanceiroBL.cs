using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

    public class ConfiguracaoFinanceiroBL : DefaultBL, IConfiguracaoFinanceiroBL {

        //Constantes
        private static ConfiguracaoFinanceiroBL _instance;

        //Propriedades
        public static ConfiguracaoFinanceiroBL getInstance => _instance = _instance ?? new ConfiguracaoFinanceiroBL();

        /// <summary>
        /// Carregar as configurações de sistema da transportadora e cachear os dados se necessário
        /// </summary>
        public ConfiguracaoFinanceiro carregar(int idOrganizacaoInf = 0, bool flagCache = true) {

            if (idOrganizacao > 0) {
                idOrganizacaoInf = idOrganizacao;
            }

            if (flagCache) {
                var cacheData = CacheService.getInstance.carregar(CacheService.CONFIGURACAO_FINANCEIRO);

                if (cacheData != null) {
                    return (ConfiguracaoFinanceiro)cacheData;
                }
            }

            var query = db.ConfiguracaoFinanceiro.AsNoTracking().Where(x => x.flagExcluido == false);

            query = idOrganizacaoInf > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoInf) : query.Where(x => x.idOrganizacao == null);

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? new ConfiguracaoFinanceiro();

            if (flagCache) {
                CacheService.getInstance.adicionar(CacheService.CONFIGURACAO_FINANCEIRO, OConfiguracao);
            }
            return OConfiguracao;
        }

        //Configurações gerais
        public IQueryable<ConfiguracaoFinanceiro> listar(int idOrganizacaoInf) {

            if (idOrganizacao > 0) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = db.ConfiguracaoFinanceiro.AsNoTracking()
                    .Where(x => x.flagExcluido == false)
                    .Include(x => x.Organizacao);

            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            return query;
        }

        /// <summary>
        /// Salvar configurações de serviços e remover  os registros anteriores.
        /// </summary>
        public bool salvar(ConfiguracaoFinanceiro OConfiguracoes) {

            OConfiguracoes.id = 0;
            OConfiguracoes.setDefaultInsertValues();

            db.ConfiguracaoFinanceiro.Add(OConfiguracoes);
            db.SaveChanges();

            bool flagSucesso = OConfiguracoes.id > 0;

            if (flagSucesso) {
                CacheService.getInstance.remover(CacheService.CONFIGURACAO_FINANCEIRO);
            }
            return flagSucesso;
        }
    }
}