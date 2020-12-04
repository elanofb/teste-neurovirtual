using System;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;
using DAL.Configuracoes.Default;

namespace BLL.Configuracoes {

    public class ConfiguracaoEmailBL : DefaultBL, IConfiguracaoEmailBL {

        //Constantes
        private static ConfiguracaoEmailBL _instance;

        //Propriedades
        private string chaveCache = "configuracao_email";

        public static ConfiguracaoEmailBL getInstance => _instance = _instance ?? new ConfiguracaoEmailBL();

        /// <summary>
        /// Carregar as configurações de sistema da transportadora e cachear os dados se necessário
        /// </summary>
        public ConfiguracaoEmail carregar(int idOrganizacaoParam = 0, bool flagCache = true) {
    
            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = User.idOrganizacao();

            }

            if (flagCache) {

                var cacheData = CacheService.getInstance.carregar(chaveCache, idOrganizacaoParam);

                if (cacheData != null) {
                    return (ConfiguracaoEmail)cacheData;
                }
            }

            var query = db.ConfiguracaoEmail.AsNoTracking().Where(x => x.flagExcluido == false);

            if (idOrganizacaoParam > 0) {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);

            } else {
                query = query.Where(x => x.idOrganizacao == null);

            }

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? ConfiguracaoEmailDefault.carregar();

            if (flagCache) {

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam);

                CacheService.getInstance.adicionar(chaveCache, OConfiguracao, idOrganizacaoParam);
            }

            return OConfiguracao;
        }


        //Configurações gerais
        public IQueryable<ConfiguracaoEmail> listar(int idOrganizacaoParam) {

            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = User.idOrganizacao();

            }

            var query = db.ConfiguracaoEmail
                            .AsNoTracking().Where(x => x.flagExcluido == false);

            if (idOrganizacaoParam > 0) {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;
        }

        /// <summary>
        /// Salvar configurações de serviços e remover  os registros anteriores.
        /// </summary>
        public bool salvar(ConfiguracaoEmail OConfiguracoes) {

            OConfiguracoes.setDefaultInsertValues();

            db.ConfiguracaoEmail.Add(OConfiguracoes);

            db.SaveChanges();

            bool flagSucesso = OConfiguracoes.id > 0;

            if (flagSucesso) {

                int? idOrganizacaoParam = OConfiguracoes.idOrganizacao;

                db.ConfiguracaoEmail.Where(x => x.flagExcluido == false && x.idOrganizacao == idOrganizacaoParam && x.id != OConfiguracoes.id)
                                    .Update(x => new ConfiguracaoEmail { flagExcluido = true });

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam.toInt());
            }

            return flagSucesso;
        }
    }
}