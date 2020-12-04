using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using BLL.Caches;
using BLL.Services;
using DAL.Configuracoes;
using EntityFramework.Extensions;

namespace BLL.Configuracoes {

    public class ConfiguracaoRotinaAutomaticaBL : DefaultBL, IConfiguracaoRotinaAutomaticaBL {

        //Constantes
        private static ConfiguracaoRotinaAutomaticaBL _instance;

        //Propriedades
        public static ConfiguracaoRotinaAutomaticaBL getInstance => _instance = _instance ?? new ConfiguracaoRotinaAutomaticaBL();

        /// <summary>
        /// Carregar as configurações de sistema da transportadora e cachear os dados se necessário
        /// </summary>
        public ConfiguracaoRotinaAutomatica carregar(int idOrganizacao, bool flagCache = true) {

            string chaveCache = string.Concat("configuracao_rotina_automatica_", idOrganizacao);

            if (flagCache) {
                var cacheData = HttpContextFactory.Current.Cache.Get(chaveCache);

                if (cacheData != null) {
                    return (ConfiguracaoRotinaAutomatica)cacheData;
                }
            }

            var query = db.ConfiguracaoRotinaAutomatica
                            .AsNoTracking().Where(x => x.flagExcluido == false);

            query = idOrganizacao > 0 ? query.Where(x => x.idOrganizacao == idOrganizacao) : query.Where(x => x.idOrganizacao == null);

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? new ConfiguracaoRotinaAutomatica();

            if (flagCache) {
                HttpContext.Current.Cache.Insert(chaveCache, OConfiguracao, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
            }
            return OConfiguracao;
        }

        //Configurações gerais
        public IQueryable<ConfiguracaoRotinaAutomatica> listar(int idOrganizacao) {

            var query = db.ConfiguracaoRotinaAutomatica
                            .AsNoTracking().Where(x => x.flagExcluido == false);

            if (idOrganizacao > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacao);
            }
            return query;
        }

        /// <summary>
        /// Salvar configurações de serviços e remover  os registros anteriores.
        /// </summary>
        public bool salvar(ConfiguracaoRotinaAutomatica OConfiguracoes) {

            OConfiguracoes.setDefaultInsertValues();

            db.ConfiguracaoRotinaAutomatica.Add(OConfiguracoes);
            db.SaveChanges();

            bool flagSucesso = OConfiguracoes.id > 0;
            
            if (flagSucesso) {

                int? idOrganizacao = OConfiguracoes.idOrganizacao;

                db.ConfiguracaoRotinaAutomatica
                    .Where(x => x.flagExcluido == false && x.idOrganizacao == idOrganizacao && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoRotinaAutomatica { flagExcluido = true });

                CacheService.getInstance.remover("configuracao_rotina_automatic");
            }
            return flagSucesso;
        }
    }
}