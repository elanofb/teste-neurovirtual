using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.ConfiguracoesEcommerce;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.ConfiguracoesEcommerce {

	public class ConfiguracaoEcommerceBL : DefaultBL, IConfiguracaoEcommerceBL {

		//Constantes
		private static IConfiguracaoEcommerceBL _instance;

        // Atributos
        private readonly string chaveCache = "configuracao_ecommerce";

		//Propriedades
		public static IConfiguracaoEcommerceBL getInstance => _instance = _instance ?? new ConfiguracaoEcommerceBL();

	    //
		public ConfiguracaoEcommerceBL(){

		}

		/// <summary>
        /// Carregar as configurações de Ecommerce da organização e cachear os dados se necessario
        /// </summary>
		public ConfiguracaoEcommerce carregar(int idOrganizacaoParam = 0, bool flagCache = true) {

		    if (idOrganizacaoParam == 0){
		        idOrganizacaoParam = User.idOrganizacao();
		    }

		    var cacheData = CacheService.getInstance.carregar<ConfiguracaoEcommerce>(chaveCache, idOrganizacaoParam);

		    if (cacheData != null && flagCache) {
		        return cacheData;
		    }

			var query = db.ConfiguracaoEcommerce
                            .Include(x => x.Organizacao).Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null);

		    query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == null);

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? this.carregarPadrao();

            if (flagCache) {
                CacheService.getInstance.adicionar(chaveCache, OConfiguracao, idOrganizacaoParam);
            }

		    return OConfiguracao;
		}

        // Carregamento Padrão
        private ConfiguracaoEcommerce carregarPadrao() {

            var OConfig = new ConfiguracaoEcommerce();

            OConfig.flagSomenteAssociados = false;

            OConfig.flagDirecionarAposIncluirProduto = false;

            OConfig.flagHabilitarCupomDesconto = false;

            OConfig.flagHabilitarFreteGratuito = false;

            return OConfig;

        }
        
		//Configuracoes gerais
		public IQueryable<ConfiguracaoEcommerce> listar(int idOrganizacao) {

			var query = db.ConfiguracaoEcommerce
                            .Include(x => x.Organizacao).Include(x => x.Organizacao.Pessoa).Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null).AsNoTracking();

    	    if (idOrganizacao > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacao);
		    }

    	    return query;
		}

		/// <summary>
        /// Salvar configuracoes de Ecommerce e remover os registros anteriores.
        /// </summary>
		public bool salvar(ConfiguracaoEcommerce OConfiguracoes) {
			
		    OConfiguracoes.cepOrigemFrete = UtilString.onlyNumber(OConfiguracoes.cepOrigemFrete);

			OConfiguracoes.setDefaultInsertValues();

			db.ConfiguracaoEcommerce.Add(OConfiguracoes);

			db.SaveChanges();

		    bool flagSucesso = OConfiguracoes.id > 0;

		    int? idOrganizacaoParam = OConfiguracoes.idOrganizacao;

		    if (flagSucesso) {

		        db.ConfiguracaoEcommerce
                    .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacao && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoEcommerce { dtExclusao = DateTime.Now });

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam.toInt());
		    }

			return (OConfiguracoes.id > 0);

		}
	}
}