using System;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Configuracoes;
using DAL.Configuracoes.Default;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Configuracoes {

    public class ConfiguracaoNotificacaoBL : DefaultBL, IConfiguracaoNotificacaoBL {

        //Constantes
        private static ConfiguracaoNotificacaoBL _instance;

        //Propriedades
        private string chaveCache = "configuracao_notificacao";

        public static ConfiguracaoNotificacaoBL getInstance => _instance = _instance ?? new ConfiguracaoNotificacaoBL();

        /// <summary>
        /// Carregar as configurações de sistema da transportadora e cachear os dados se necessário
        /// </summary>
        public ConfiguracaoNotificacao carregar(int idOrganizacaoParam = 0, bool flagCache = true) {

            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = User.idOrganizacao();
            }

            if (flagCache) {
                var cacheData = CacheService.getInstance.carregar<ConfiguracaoNotificacao>(chaveCache, idOrganizacaoParam);

                if (cacheData != null) {
                    return cacheData;
                }
            }

            var query = db.ConfiguracaoNotificacao.AsNoTracking().Where(x => x.flagExcluido == false);

            if (idOrganizacaoParam > 0) {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);

            } else {
                query = query.Where(x => x.idOrganizacao == null);

            }

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? new ConfiguracaoNotificacao();

            ConfiguracaoNotificacaoDefault.completarInformacoes(OConfiguracao);


            if (flagCache) {

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam);

                CacheService.getInstance.adicionar(chaveCache, OConfiguracao, idOrganizacaoParam);
            }

            return OConfiguracao;
        }

        //Configurações gerais
        public IQueryable<ConfiguracaoNotificacao> listar(int idOrganizacaoInfo) {

            var query = db.ConfiguracaoNotificacao
                            .AsNoTracking().Where(x => x.flagExcluido == false);

            if (idOrganizacaoInfo > 0) {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoInfo);

            }

            return query;
        }

        /// <summary>
        /// Salvar configurações de serviços e remover  os registros anteriores.
        /// </summary>
        public bool salvar(ConfiguracaoNotificacao OConfiguracoes) {

            OConfiguracoes.setDefaultInsertValues();

            db.ConfiguracaoNotificacao.Add(OConfiguracoes);

            db.SaveChanges();

            bool flagSucesso = OConfiguracoes.id > 0;

            if (flagSucesso) {

                int? idOrganizacaoInfo = OConfiguracoes.idOrganizacao;

                db.ConfiguracaoNotificacao
                    .Where(x => x.flagExcluido == false && x.idOrganizacao == idOrganizacaoInfo && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoNotificacao { flagExcluido = true });

                CacheService.getInstance.remover(chaveCache, idOrganizacaoInfo.toInt());
            }
            return flagSucesso;
        }
    }
}