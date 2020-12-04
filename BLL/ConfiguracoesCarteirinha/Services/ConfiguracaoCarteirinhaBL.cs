using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Caching;
using BLL.Caches;
using BLL.Services;
using DAL.ConfiguracoesAreaAssociado;
using DAL.ConfiguracoesCateirinha;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.ConfiguracoesCarteirinha {

	public class ConfiguracaoCarteirinhaBL: DefaultBL, IConfiguracaoCarteirinhaBL {

		//Constantes
		private static IConfiguracaoCarteirinhaBL _instance;

        // Atributos
        private readonly string chaveCache = "configuracao_carteirinha";

		//Propriedades
		public static IConfiguracaoCarteirinhaBL getInstance => _instance = _instance ?? new ConfiguracaoCarteirinhaBL();

	    //
		public ConfiguracaoCarteirinhaBL(){

		}

		/// <summary>
        /// Carregar as configurações de Carteirinha da organização e cachear os dados se necessario
        /// </summary>
		public ConfiguracaoCarteirinha carregar(int idOrganizacaoParam = 0, bool flagCache = true) {

		    if (idOrganizacaoParam == 0){

		        idOrganizacaoParam = User.idOrganizacao();
		    }

		    var cacheData = CacheService.getInstance.carregar<ConfiguracaoCarteirinha>(chaveCache, idOrganizacaoParam);
            
		    if (cacheData != null && flagCache) {
		        return cacheData;
		    }

			var query = db.ConfiguracaoCarteirinha
                            .Include(x => x.Organizacao).Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null).AsNoTracking();

		    query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == null);

		    var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? this.carregarPadrao();

            if (flagCache) {
                CacheService.getInstance.adicionar(chaveCache, OConfiguracao, idOrganizacaoParam);
            }

		    return OConfiguracao;
		}

        // Carregamento Padrão
        private ConfiguracaoCarteirinha carregarPadrao() {

            var OConfig = new ConfiguracaoCarteirinha();

            OConfig.htmlCarteirinha = "";

            OConfig.qtdeMesesValidade = 12;

            return OConfig;

        }
        
		//Configuracoes gerais
		public IQueryable<ConfiguracaoCarteirinha> listar(int idOrganizacaoParam) {

		    if (idOrganizacaoParam == 0){

		        idOrganizacaoParam = User.idOrganizacao();
		    }

			var query = db.ConfiguracaoCarteirinha
                            .Include(x => x.Organizacao).Include(x => x.Organizacao.Pessoa).Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null).AsNoTracking();

    	    if (idOrganizacaoParam > 0) {

		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);

		    }

    	    return query;
		}

		/// <summary>
        /// Salvar configuracoes de Área do Associado e remover  os registros anteriores.
        /// </summary>
		public bool salvar(ConfiguracaoCarteirinha OConfiguracoes) {
			
			OConfiguracoes.setDefaultInsertValues();

			db.ConfiguracaoCarteirinha.Add(OConfiguracoes);

			db.SaveChanges();

		    bool flagSucesso = OConfiguracoes.id > 0;

		    int? idOrganizacaoParam = OConfiguracoes.idOrganizacao;

		    if (flagSucesso) {

		        db.ConfiguracaoCarteirinha
                    .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacao && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoCarteirinha { dtExclusao = DateTime.Now });

                CacheService.getInstance.remover("configuracao_carteirinha", idOrganizacaoParam.toInt());
		    }

			return (OConfiguracoes.id > 0);

		}
	}
}