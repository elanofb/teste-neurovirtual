using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.ConfiguracoesRedesSociais;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.ConfiguracoesRedesSociais {

	public class ConfiguracaoRedesSociaisBL: DefaultBL, IConfiguracaoRedesSociaisBL {

		//Constantes
		private static IConfiguracaoRedesSociaisBL _instance;

        // Atributos
        private readonly string chaveCache = "configuracao_redessociais";

		//Propriedades
		public static IConfiguracaoRedesSociaisBL getInstance => _instance = _instance ?? new ConfiguracaoRedesSociaisBL();

	    //
		public ConfiguracaoRedesSociaisBL(){

		}

		/// <summary>
        /// Carregar as configurações de RedesSociais da organização e cachear os dados se necessario
        /// </summary>
		public ConfiguracaoRedesSociais carregar(int idOrganizacaoParam = 0, bool flagCache = true) {

		    if (idOrganizacaoParam == 0){
		        idOrganizacaoParam = User.idOrganizacao();
		    }

		    var cacheData = CacheService.getInstance.carregar<ConfiguracaoRedesSociais>(chaveCache, idOrganizacaoParam);

		    if (cacheData != null && flagCache) {
		        return cacheData;
		    }

			var query = db.ConfiguracaoRedesSociais
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
        private ConfiguracaoRedesSociais carregarPadrao() {

            var OConfig = new ConfiguracaoRedesSociais();

            // Configurar padrões

            return OConfig;

        }
        
		//Configuracoes gerais
		public IQueryable<ConfiguracaoRedesSociais> listar(int idOrganizacaoParam) {

			var query = db.ConfiguracaoRedesSociais
                            .Include(x => x.Organizacao).Include(x => x.Organizacao.Pessoa).Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null).AsNoTracking();

    	    if (idOrganizacao > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
		    }

    	    return query;
		}

		/// <summary>
        /// Salvar configuracoes de RedesSociais e remover os registros anteriores.
        /// </summary>
		public bool salvar(ConfiguracaoRedesSociais OConfiguracoes) {
			
			OConfiguracoes.setDefaultInsertValues();

			db.ConfiguracaoRedesSociais.Add(OConfiguracoes);

			db.SaveChanges();

		    bool flagSucesso = OConfiguracoes.id > 0;

		    int? idOrganizacaoParam = OConfiguracoes.idOrganizacao;

		    if (flagSucesso) {

		        db.ConfiguracaoRedesSociais
                    .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacaoParam && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoRedesSociais { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id() });

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam.toInt());
		    }

			return (OConfiguracoes.id > 0);

		}
	}
}