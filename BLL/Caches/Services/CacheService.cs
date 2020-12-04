using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using DAL.Permissao.Security.Extensions;

namespace BLL.Caches {

    public class CacheService {

        //Atributos

        //Propriedades

        //Constantes
        public static string LISTA_RECURSOS = "lista_recursos";

        public static string LISTA_PERMISSOES = "lista_permissoes";

        public static string CONFIGURACAO_GERAL = "configuracao_geral";

        public static string CONFIGURACAO_CONTRIBUICAO = "configuracao_contribuicao";

        public static string CONFIGURACAO_FINANCEIRO = "configuracao_financeiro";

        public static string CONFIGURACAO_PORTAL = "configuracao_portal";

        public static string TIPO_ASSOCIADO_DD_SIMPLES = "tipo_associado_dd_simples";

        public static string AREA_ATUACAO_DD_SIMPLES = "area_atuacao_dd_simples";

        public static string TIPO_DOCUMENTO_DD_SIMPLES = "tipo_documento_dd_simples";

        public static string ORGAO_CLASSE_DD_SIMPLES = "orgao_classe_dd_simples";

        public static string RAMO_ATIVIDADE_DD_SIMPLES = "ramo_atividade_dd_simples";

        public static string SETOR_ATUACAO_DD_SIMPLES = "setor_atuacao_dd_simples";

        public static string MEIO_DIVULGACAO_DD_SIMPLES = "meio_divulgacao_dd_simples";
        

        //Atributos
        private static CacheService _instance;

        //Propriedades
        public static CacheService getInstance => _instance = _instance ?? new CacheService();

        /// <summary>
        /// Definir prefixo das chaves a partir do id da organizacao
        /// </summary>
        private string prefixoKey(int idOrganizacao) {

            string prefixoKey = $"org_{idOrganizacao}";

            return prefixoKey;
        }

        /// <summary>
        /// Montagem para uma chave de chave
        /// </summary>
        private string keyCache(int idOrganizacao, string key) {

            string keyRetorno = $"{this.prefixoKey(idOrganizacao)}_{key}";

            return keyRetorno;
        }

        /// <summary>
        /// Carregar um item salvo no cache
        /// </summary>
        public object carregar(string key, int? idOrganizacaoParam = null) {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            if (idOrganizacao > 0 && idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }
            
            key = this.keyCache(idOrganizacaoParam.toInt(), key);

            var cacheData = HttpContextFactory.Current.Cache.Get(key);

            return cacheData;
        }

        /// <summary>
        /// Carregar um registro a partir do cache
        /// </summary>
        public T carregar<T>(string key, int idOrganizacaoParam = 0) where T : class {

            int idOrganizacao = idOrganizacaoParam > 0? idOrganizacaoParam: HttpContext.Current.User.idOrganizacao();

            key = this.keyCache(idOrganizacao, key);

            var cacheData = HttpContextFactory.Current.Cache.Get(key);

            return cacheData as T;
        }

        /// <summary>
        /// Carregar um registro a partir do cache
        /// </summary>
        public T carregarSemOrganizacao<T>(string key) where T : class {

            var cacheData = HttpContextFactory.Current.Cache.Get(key);

            return cacheData as T;
        }

        /// <summary>
        /// Adicionar itens em cache
        /// </summary>
        public object adicionar(string key, object valor, int idOrganizacaoParam = 0) {

            int idOrganizacao = idOrganizacaoParam > 0? idOrganizacaoParam : HttpContextFactory.Current.User.idOrganizacao();

            key = this.keyCache(idOrganizacao, key);

            if (HttpContextFactory.Current.Cache.Get(key) != null) {

                HttpContextFactory.Current.Cache.Remove(key);

            }

            HttpContextFactory.Current.Cache.Add(key, valor, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            
            return carregar(key);
        }

        /// <summary>
        /// Adicionar itens em cache
        /// </summary>
        public object adicionarSemOrganizacao(string key, object valor) {

            if (HttpContextFactory.Current.Cache.Get(key) != null) {

                HttpContextFactory.Current.Cache.Remove(key);

            }

            HttpContextFactory.Current.Cache.Add(key, valor, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            
            return carregar(key);
        }


        /// <summary>
        /// Limpar todas as chaves
        /// </summary>
        public void limparCacheOrganizacao(int? idOrganizacaoParam, string key = "") {

            var idOrganizacao = idOrganizacaoParam ?? HttpContextFactory.Current.User.idOrganizacao();

            string prefixoKey = key.isEmpty() ? this.prefixoKey(idOrganizacao) : this.keyCache(idOrganizacao, key);

            foreach (DictionaryEntry entry in HttpContextFactory.Current.Cache) {

                string cacheKey = (string)entry.Key;

                if (cacheKey.StartsWith(prefixoKey)) {

                    HttpContextFactory.Current.Cache.Remove(cacheKey);

                }
            }

            CacheService.getInstance.removerSemOrganizacao(CacheService.LISTA_RECURSOS);
        }

        /// <summary>
        /// Limpar todas as chaves
        /// </summary>
        public void limparConfiguracoes() {

            foreach (DictionaryEntry entry in HttpContextFactory.Current.Cache) {

                string cacheKey = (string)entry.Key;

                HttpContextFactory.Current.Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Limpar uma chave de cache 
        /// </summary>
        public void remover(string key = "", int idOrganizacaoParam = 0) {

            if (string.IsNullOrEmpty(key)) {
                return;
            }

            int idOrganizacao = idOrganizacaoParam > 0? idOrganizacaoParam : HttpContextFactory.Current.User.idOrganizacao();

            key = this.keyCache(idOrganizacao, key);

            if (HttpContextFactory.Current.Cache.Get(key) != null) {
                HttpContextFactory.Current.Cache.Remove(key);
            }
        }

        /// <summary>
        /// Limpar uma chave de cache 
        /// </summary>
        public void removerSemOrganizacao(string key = "") {

            if (string.IsNullOrEmpty(key)) {
                return;
            }

            if (HttpContextFactory.Current.Cache.Get(key) != null) {

                HttpContextFactory.Current.Cache.Remove(key);

            }
        }
    }
}
