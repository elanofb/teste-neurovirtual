using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.ConfiguracoesScripts;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.ConfiguracoesScripts {

	public class ConfiguracaoScriptsBL: DefaultBL, IConfiguracaoScriptsBL {

		//Constantes
		private static IConfiguracaoScriptsBL _instance;

        // Atributos
        private readonly string chaveCache = "configuracao_scripts";

		//Propriedades
		public static IConfiguracaoScriptsBL getInstance => _instance = _instance ?? new ConfiguracaoScriptsBL();

	    //
		public ConfiguracaoScriptsBL(){

		}

		/// <summary>
        /// Carregar as configurações de Scripts da organização e cachear os dados se necessario
        /// </summary>
		public ConfiguracaoScripts carregar(int idOrganizacaoParam = 0, bool flagCache = true) {

		    if (idOrganizacaoParam == 0){
		        idOrganizacaoParam = User.idOrganizacao();
		    }

		    var cacheData = CacheService.getInstance.carregar<ConfiguracaoScripts>(chaveCache, idOrganizacaoParam);

		    if (cacheData != null && flagCache) {
		        return cacheData;
		    }

			var query = db.ConfiguracaoScripts
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
        private ConfiguracaoScripts carregarPadrao() {

            var OConfig = new ConfiguracaoScripts();

            OConfig.googleAnalylics = "";

            OConfig.googleMaps = "";

            OConfig.scriptFroala = "";

            OConfig.scriptChat = "";

            return OConfig;

        }
        
		//Configuracoes gerais
		public IQueryable<ConfiguracaoScripts> listar(int idOrganizacaoParam) {

			var query = db.ConfiguracaoScripts
                            .Include(x => x.Organizacao)
                            .Include(x => x.Organizacao.Pessoa)
                            .Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null).AsNoTracking();

    	    if (idOrganizacaoParam > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
		    }

    	    return query;
		}

		/// <summary>
        /// Salvar configuracoes de Scripts e remover os registros anteriores.
        /// </summary>
		public bool salvar(ConfiguracaoScripts OConfiguracoes) {
			
			OConfiguracoes.setDefaultInsertValues();

			db.ConfiguracaoScripts.Add(OConfiguracoes);

			db.SaveChanges();

		    bool flagSucesso = OConfiguracoes.id > 0;

		    int? idOrganizacaoParam = OConfiguracoes.idOrganizacao;

		    if (flagSucesso) {

		        db.ConfiguracaoScripts
                    .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacaoParam && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoScripts { dtExclusao = DateTime.Now });

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam.toInt());
		    }

			return (OConfiguracoes.id > 0);

		}
	}
}