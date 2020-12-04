using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Caching;
using BLL.Caches;
using BLL.Services;
using EntityFramework.Extensions;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;

namespace BLL.ConfiguracoesAssociados {

	public class ConfiguracaoAssociadoPJBL: DefaultBL, IConfiguracaoAssociadoPJBL {

		//Constantes
		private static IConfiguracaoAssociadoPJBL _instance;

        // Atributos
        private readonly string chaveCache = "configuracao_associado_pj";

		//Propriedades
		public static IConfiguracaoAssociadoPJBL getInstance => _instance = _instance ?? new ConfiguracaoAssociadoPJBL();

	    //
		public ConfiguracaoAssociadoPJBL(){

		}

		/// <summary>
        /// Carregar as configurações de Associados PJ da organização e cachear os dados se necessario
        /// </summary>
		public ConfiguracaoAssociadoPJ carregar(int idOrganizacaoParam = 0, bool flagCache = true) {

		    if (idOrganizacaoParam == 0){

		        idOrganizacaoParam = User.idOrganizacao();

		    }

		    var cacheData = CacheService.getInstance.carregar<ConfiguracaoAssociadoPJ>(chaveCache, idOrganizacaoParam);

		    if (cacheData != null && flagCache) {
		        return cacheData;
		    }

			var query = db.ConfiguracaoAssociadoPJ
                            .Include(x => x.Organizacao)
			                .Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null).AsNoTracking();

		    query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == null);

		    var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? this.carregarPadrao();

            if (flagCache) {
                CacheService.getInstance.adicionar(chaveCache, OConfiguracao, idOrganizacaoParam);
            }

		    return OConfiguracao;
		}

        // Carregamento Padrão
        private ConfiguracaoAssociadoPJ carregarPadrao() {

            var OConfig = new ConfiguracaoAssociadoPJ();

            OConfig.flagHabilitado = true;

            OConfig.flagAbaContato = true;

            OConfig.flagAbaTitulos = false;

            OConfig.flagAbaPedidos = true;

            OConfig.flagAbaContribuicoes = true;

            OConfig.flagAbaEventos = true;

            OConfig.flagAbaAnuncios = false;

            OConfig.flagAbaAreasAtuacao = false;

            OConfig.flagAbaRepresentantes = true;

            OConfig.flagAbaListaEmails = false;

            OConfig.flagAbaDependentes = false;

            OConfig.flagAbaInstituicoes = false;
            
            OConfig.flagAbaAbrangencia = false;

            OConfig.flagAbaCarteirinha = false;

            OConfig.flagAbaVeiculos = false;

            OConfig.flagPermitirCNPJVazio = false;

            OConfig.flagPermitirCNPJDuplicado = false;

            return OConfig;
        }
        
		//Configuracoes gerais
		public IQueryable<ConfiguracaoAssociadoPJ> listar(int idOrganizacaoParam) {

            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = idOrganizacao;

            }


			var query = db.ConfiguracaoAssociadoPJ
                            .Include(x => x.Organizacao).Include(x => x.Organizacao.Pessoa).Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null).AsNoTracking();

    	    if (idOrganizacaoParam > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
		    }

    	    return query;
		}

		/// <summary>
        /// Salvar configuracoes de Associados PJ e remover  os registros anteriores.
        /// </summary>
		public bool salvar(ConfiguracaoAssociadoPJ OConfiguracoes) {
			
			OConfiguracoes.setDefaultInsertValues();

			db.ConfiguracaoAssociadoPJ.Add(OConfiguracoes);

			db.SaveChanges();

		    bool flagSucesso = OConfiguracoes.id > 0;

		    int? idOrganizacao = OConfiguracoes.idOrganizacao;

		    if (flagSucesso) {

		        db.ConfiguracaoAssociadoPJ
                    .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacao && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoAssociadoPJ { dtExclusao = DateTime.Now });

			    CacheService.getInstance.remover(chaveCache, idOrganizacao.toInt());
		    }

			return (OConfiguracoes.id > 0);

		}
	}
}