using System;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Configuracoes;
using EntityFramework.Extensions;
using System.Data.Entity;
using DAL.Permissao.Security.Extensions;

namespace BLL.Configuracoes {

    public class ConfiguracaoSistemaBL : DefaultBL, IConfiguracaoSistemaBL {

        //Constantes
        private static IConfiguracaoSistemaBL _instance;

        //Propriedades
        private string chaveCache = "configuracao_sistema";

        //Servicos
        public static IConfiguracaoSistemaBL getInstance => _instance = _instance ?? new ConfiguracaoSistemaBL();


        //
        public ConfiguracaoSistemaBL() {
        }

        /// <summary>
        /// Carregar as configurações de sistema da transportadora e cachear os dados se necessario
        /// </summary>
        public ConfiguracaoSistema carregar(int idOrganizacaoParam = 0, bool flagCache = true) {

            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = User.idOrganizacao();

            }

            var cacheData = CacheService.getInstance.carregar<ConfiguracaoSistema>(chaveCache, idOrganizacaoParam);

            if (cacheData != null && flagCache) {
                return cacheData;
            }

            var query = db.ConfiguracaoSistema
                            .Include(x => x.Organizacao)
                            .Include(x => x.UsuarioSistema)
                            .Where(x => x.flagExcluido == false)
                            .AsNoTracking();

            if (idOrganizacaoParam > 0) {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);

            } else {
                query = query.Where(x => x.idOrganizacao == null);

            }

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? carregarPadrao();

            if (flagCache) {

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam);

                CacheService.getInstance.adicionar(chaveCache, OConfiguracao, idOrganizacaoParam);
            }

            return OConfiguracao;
        }

        /// <summary>
        /// Carregar as configurações de sistema através da chave de acesso da API e cachear os dados se necessario
        /// </summary>
		public ConfiguracaoSistema carregarParaApi(string token) {


            var query = db.ConfiguracaoSistema
                            .Where(x => x.flagExcluido == false)
                            .AsNoTracking();

            query = query.Where(x => x.apiChaveAcesso.Equals(token));

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? carregarPadrao();

            return OConfiguracao;
        }

        /// <summary>
        /// Caso nao encontre nenhuma configuracao realizada carrega a padrão
        /// </summary>
        /// <returns></returns>
	    public ConfiguracaoSistema carregarPadrao() {

            ConfiguracaoSistema Config = new ConfiguracaoSistema();

            Config.idOrganizacao = 0;

            Config.htmlLogoTopo = "<b>Sinctec</b>Associatec";

            Config.htmlLogoTopoMini = "<b>Sinc</b>Tec";

            Config.temaInterface = "skin-blue";

            Config.tituloSistema = "Associatec";

            Config.nomeEmpresaResumo = "Associatec v2";

            Config.nomeEmpresaCompleto = "Associatec v2 LTDA";

            Config.dominios = "";

            Config.flagBgLoginCustomizado = false;

            return Config;
        }

        //Configuracoes gerais
        public IQueryable<ConfiguracaoSistema> listar(int idOrganizacaoParam) {

            var query = db.ConfiguracaoSistema
                            .Include(x => x.Organizacao)
                            .Include(x => x.Organizacao.Pessoa)
                            .Include(x => x.UsuarioSistema)
                            .Where(x => x.flagExcluido == false).AsNoTracking();

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;
        }

        /// <summary>
        /// Salvar configuracoes de servicos e remover  os registros anteriores.
        /// </summary>
        public bool salvar(ConfiguracaoSistema OConfiguracoes) {

            OConfiguracoes.setDefaultInsertValues();

            db.ConfiguracaoSistema.Add(OConfiguracoes);

            db.SaveChanges();

            bool flagSucesso = OConfiguracoes.id > 0;

            int? idOrganizacaoParam = OConfiguracoes.idOrganizacao;

            if (flagSucesso) {

                db.ConfiguracaoSistema
                    .Where(x => x.flagExcluido == false && x.idOrganizacao == idOrganizacaoParam && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoSistema { flagExcluido = true });

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam.toInt());
            }

            return (OConfiguracoes.id > 0);
        }
    }
}